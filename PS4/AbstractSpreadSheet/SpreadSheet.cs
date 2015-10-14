/*
Author: Trung Le
Date: 09/29/2015
Purpose: SpreadSheet class implementatio
THIS IS THE PS5 BRANCH
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;


namespace SS
{

    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a valid cell name if and only if:
    ///   (1) its first character is an underscore or a letter
    ///   (2) its remaining characters (if any) are underscores and/or letters and/or digits
    /// Note that this is the same as the definition of valid variable from the PS3 Formula class.
    /// 
    /// For example, "x", "_", "x2", "y_15", and "___" are all valid cell  names, but
    /// "25", "2x", and "" are not.  Cell names are case sensitive, so "x" and "X" are
    /// different cell names.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  (This
    /// means that a spreadsheet contains an infinite number of cells.)  In addition to 
    /// a name, each cell has a contents and a value.  The distinction is important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {

        private DependencyGraph dependencies;
        private Dictionary<string, Cell> cellList;
        bool changed;

        /// <summary>
        /// Represents a cell
        /// Can contain either a Formula, a string, a FormulaError or a double
        /// </summary>
        private class Cell
        {
            string stringContent = null;
            double doubleContent = double.PositiveInfinity;
            double doubleContent1 = double.PositiveInfinity;
            Formula formulaContent = null;
            FormulaError formulaErrorContent;

            /// <summary>
            /// To represent a cell holding a string
            /// </summary>
            /// <param name="content"></param>
            public Cell(String content)
            {
                stringContent = content;
            }

            /// <summary>
            /// To represent a cell holding a double
            /// </summary>
            /// <param name="content"></param>
            public Cell(double content)
            {
                doubleContent1 = doubleContent = content;
            }

            /// <summary>
            /// To represent a cell holding a formula. 
            /// If the formula can be evaluated then assign the result to doubleContent
            /// Else, assign formulaError to formulaErrorContent
            /// </summary>
            /// <param name="content"></param>
            public Cell(Formula content, Func<string, double> lookup)
            {
                formulaContent = content;

                try
                {
                    doubleContent = (double)content.Evaluate(lookup);
                }
                catch (InvalidCastException)
                {
                    formulaErrorContent = (FormulaError)content.Evaluate(lookup);
                }
            }

            /// <summary>
            /// Returns the raw content of the cell
            /// </summary>
            /// <returns></returns>
            public object getContent()
            {
                if (stringContent != null)
                {
                    return stringContent;
                }
                else if (doubleContent1 != double.PositiveInfinity)
                {
                    return doubleContent;
                }
                else
                {
                    return formulaContent;
                }
            }

            /// <summary>
            /// Return the value of the cell
            /// Returns the double if it exists,
            /// If not then return formulaError
            /// </summary>
            /// <returns></returns>
            public object getValue()
            {
                if (doubleContent != double.PositiveInfinity)
                {
                    return doubleContent;
                }
                else
                {
                    return formulaErrorContent;
                }
            }
        }

        /// <summary>
        /// Zero argument constructor
        /// Creates an empty spreadsheet that imposes no extra validity conditions, normalizes every cell name to itself, and has version "default".
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            cellList = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// Three-argument constructor. 
        /// Creates an empty spreadsheet. Allows the user to provide a validity delegate (first parameter), 
        /// a normalization delegate (second parameter), and a version (third parameter)
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cellList = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// Four-argument constructor to the Spreadsheet class. Allows the user to provide a string representing a path to a file (first parameter),
        /// a validity delegate (second parameter), a normalization delegate (third parameter), and a version (fourth parameter). 
        /// Reads a saved spreadsheet from a file (see the Save method) and use it to construct a new spreadsheet. 
        /// The new spreadsheet should use the provided validity delegate, normalization delegate, and version.
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(string pathToFile, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            cellList = new Dictionary<string, Cell>();
            dependencies = new DependencyGraph();

            if (pathToFile == null)
            {
                throw new SpreadsheetReadWriteException("Path name cannot be null");
            }

            if (!(GetSavedVersion(pathToFile).Equals(version)))
            {
                throw new SpreadsheetReadWriteException("Version of the file from " + pathToFile + " does not match the version from the parameter");
            }

            try
            {
                using (XmlReader reader = XmlReader.Create(pathToFile))
                {
                    while (reader.Read())
                    {

                    }
                }
            }
            catch (InvalidNameException)
            {
                throw new SpreadsheetReadWriteException("Invalid name encountered");
            }
            catch (FormulaFormatException)
            {
                throw new SpreadsheetReadWriteException("Invalid formula encountered");
            }
            catch (CircularException)
            {
                throw new SpreadsheetReadWriteException("Circular dependency found");
            }
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return changed;
            }
            protected set
            {
                changed = value;
            }
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            return cellList.Keys;
        }


        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(String name)
        {
            if (string.IsNullOrEmpty(name) || !isVariable(name))
                throw new InvalidNameException();

            Cell c;

            if (cellList.TryGetValue(name, out c))
            {
                return c.getContent();
            }

            else
            {
                return null;
            }
        }


        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<String> SetCellContents(String name, double number)
        {
            if (string.IsNullOrEmpty(name) || !isVariable(name))
            {
                throw new InvalidNameException();
            }

            try
            {
                cellList.Add(name, new Cell(number));
            }
            catch (ArgumentException)
            {
                cellList.Remove(name);
                cellList.Add(name, new Cell(number));
            }

            HashSet<String> cellsToRecalculate = new HashSet<string>();

            foreach (String s in GetCellsToRecalculate(name))
                cellsToRecalculate.Add(s);

            return cellsToRecalculate;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<String> SetCellContents(String name, String text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(name) || !isVariable(name))
            {
                throw new InvalidNameException();
            }

            try
            {
                cellList.Add(name, new Cell(text));
            }
            catch (ArgumentException)
            {
                cellList.Remove(name);

                if (text != "")
                {
                    cellList.Add(name, new Cell(text));
                }
            }

            if (text == "")
            {
                cellList.Remove(name);
            }

            HashSet<String> cellsToRecalculate = new HashSet<string>();

            foreach (String s in GetCellsToRecalculate(name))
                cellsToRecalculate.Add(s);

            return cellsToRecalculate;

            return new HashSet<String> { name };
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<String> SetCellContents(String name, Formula formula)
        {
            IEnumerable<String> dependees = dependencies.GetDependees(name);

            dependencies.ReplaceDependents(name, formula.GetVariables());

            if (formula == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(name) || !isVariable(name))
            {
                throw new InvalidNameException();
            }

            try
            {
                if (cellList.ContainsKey(name))
                {
                    cellList[name] = new Cell(formula, LookupValue); ;
                }

                else
                {
                    cellList.Add(name, new Cell(formula, LookupValue));
                }


                if (dependencies.HasDependees(name))
                {
                    HashSet<String> cellsToRecalculate = new HashSet<string>();

                    foreach (String s in GetCellsToRecalculate(name))
                        cellsToRecalculate.Add(s);

                    return cellsToRecalculate;
                }

                foreach (string s in formula.GetVariables())
                {
                    if (s.Equals(name))
                    {
                        throw new CircularException();
                    }
                }
            }
            catch (CircularException)
            {
                dependencies.ReplaceDependees(name, dependees);
                throw new CircularException();
            }

            return new HashSet<String> { name };
        }


        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            if (!isVariable(name))
            {
                throw new InvalidNameException();
            }

            if (dependencies.HasDependees(name))
            {
                return dependencies.GetDependees(name);
            }

            else
            {
                return new HashSet<string>();
            }
        }


        /// <summary>
        /// Requires that names be non-null.  Also requires that if names contains s,
        /// then s must be a valid non-null cell name.
        /// 
        /// If any of the named cells are involved in a circular dependency,
        /// throws a CircularException.
        /// 
        /// Otherwise, returns an enumeration of the names of all cells whose values must
        /// be recalculated, assuming that the contents of each cell named in names has changed.
        /// The names are enumerated in the order in which the calculations should be done.  
        /// 
        /// For example, suppose that 
        /// A1 contains 5
        /// B1 contains 7
        /// C1 contains the formula A1 + B1
        /// D1 contains the formula A1 * C1
        /// E1 contains 15
        /// 
        /// If A1 and B1 have changed, then A1, B1, and C1, and D1 must be recalculated,
        /// and they must be recalculated in either the order A1,B1,C1,D1 or B1,A1,C1,D1.
        /// The method will produce one of those enumerations.
        /// 
        /// </summary>
        protected IEnumerable<String> GetCellsToRecalculate(ISet<String> names)
        {
            LinkedList<String> changed = new LinkedList<String>();
            HashSet<String> visited = new HashSet<String>();
            foreach (String name in names)
            {
                if (!visited.Contains(name))
                {
                    Visit(name, name, visited, changed);
                }
            }
            return changed;
        }


        /// <summary>
        /// A convenience method for invoking the other version of GetCellsToRecalculate
        /// with a singleton set of names.  See the other version for details.
        /// </summary>
        protected IEnumerable<String> GetCellsToRecalculate(String name)
        {
            return GetCellsToRecalculate(new HashSet<String>() { name });
        }

        /// <summary>
        /// A helper for the GetCellsToRecalculate method.
        /// 
        /// This method steps through and finds all the dependents of name. If a dependent of name equals start then there is a circular dependency
        /// </summary>
        private void Visit(String start, String name, ISet<String> visited, LinkedList<String> changed)
        {
            visited.Add(name);
            foreach (String n in GetDirectDependents(name))
            {
                if (n.Equals(start))
                {
                    throw new CircularException();
                }
                else if (!visited.Contains(n))
                {
                    Visit(start, n, visited, changed);
                }
            }
            changed.AddFirst(name);
        }

        /// <summary>
        /// Check to see if a string is a valid variable.
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Boolean</returns>
        private static Boolean isVariable(String s)
        {
            return Regex.IsMatch(s, "^([a-zA-Z]+[1-9][0-9]*)$");
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(String name)
        {
            if (string.IsNullOrEmpty(name) || !isVariable(name))
                throw new InvalidNameException();

            Cell c;

            if (cellList.TryGetValue(name, out c))
            {
                return c.getValue();
            }

            else
            {
                return null;
            }
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<String> SetContentsOfCell(String name, String content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(name) || !isVariable(name))
            {
                throw new InvalidNameException();
            }

            double valueDouble;
            if (Double.TryParse(content, out valueDouble))
            {
                SetCellContents(name, valueDouble);

                ISet<String> cellsToRecalculate = new HashSet<String>();

                foreach (String s in GetCellsToRecalculate(name))
                    cellsToRecalculate.Add(s);

                changed = true;
                return cellsToRecalculate;
            }

            if (content.StartsWith("="))
            {
                string temp = content.Substring(1);

                Formula formula = new Formula(temp, Normalize, IsValid);

                try
                {
                    Changed = true;
                    ISet<String> cellsToRecalculate = new HashSet<String>();
                    SetCellContents(name, formula);

                    foreach (String s in GetCellsToRecalculate(name))
                        cellsToRecalculate.Add(s);

                    changed = true;
                    return cellsToRecalculate;
                }
                catch (CircularException)
                {
                    Changed = false;
                    throw new CircularException();
                }
            }
            else
            {
                SetCellContents(name, content);
                ISet<String> cellsToRecalculate = new HashSet<String>();
                foreach (String s in GetCellsToRecalculate(name))
                    cellsToRecalculate.Add(s);

                changed = true;
                return cellsToRecalculate;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns>The double value of cell named s</returns>
        private double LookupValue(string s)
        {
            Cell c;

            if (cellList.TryGetValue(s, out c))
            {
                if (c.getValue() is double)
                {
                    return (double)c.getValue();
                }

                else
                {
                    throw new ArgumentException();
                }

            }
            else
                throw new ArgumentException();
        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(String filename)
        {
            if (filename.Equals(null))
                throw new SpreadsheetReadWriteException("Filename cannot be null");

            if (filename.Equals(""))
                throw new SpreadsheetReadWriteException("Filename cannot be empty");

            try
            {
                using (XmlReader doc = XmlReader.Create(filename))
                {
                    try
                    {
                        doc.Read(); 
                    }
                    catch (XmlException)
                    {
                        throw new SpreadsheetReadWriteException("Spreadsheet file: " + filename + " was empty.");
                    }

                    try
                    {
                        if (doc.Read())
                        {
                            if (doc.IsStartElement())
                            {
                                if (doc.Name == "Spreadsheet")
                                {
                                    return doc.GetAttribute("Version");
                                }
                                else
                                    throw new SpreadsheetReadWriteException("First element in " + filename + " was not \"Spreadsheet\".");
                            }
                            else
                                throw new SpreadsheetReadWriteException("First element was not an opening tag.");
                        }
                        else
                            throw new SpreadsheetReadWriteException("No elements in file.");
                    }
                    catch (XmlException)
                    {
                        throw new SpreadsheetReadWriteException("Spreadsheet file: " + filename + " cannot be read.");
                    }
                }
            }
            catch (ArgumentException)
            {
                throw new SpreadsheetReadWriteException("Filename was invalid.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Directory not found.");
            }
            catch (System.IO.FileNotFoundException)
            {
                throw new SpreadsheetReadWriteException("File not found.");
            }


        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(String filename)
        {
            Cell temp;
            Object content;

            if (filename.Equals(null))
                throw new SpreadsheetReadWriteException("Filename cannot be null");

            if (filename.Equals(""))
                throw new SpreadsheetReadWriteException("Filename cannot be empty");

            try
            {
                using (XmlWriter doc = XmlWriter.Create(filename))
                {
                    doc.WriteStartDocument();
                    doc.WriteStartElement("Spreadsheet");
                    doc.WriteAttributeString("Version", Version);

                    foreach (String s in GetNamesOfAllNonemptyCells())
                    {
                        cellList.TryGetValue(s, out temp);
                        content = temp.getContent();

                        if (content is String)
                        {
                            String contentsOfCell = (String)content;

                            doc.WriteStartElement("Cell");
                            doc.WriteElementString("Name: ", s);
                            doc.WriteElementString("Content: ", contentsOfCell);
                            doc.WriteEndElement();
                        }
                        else if (content is double)
                        {
                            double contentsofCell = (double)content;

                            doc.WriteStartElement("Cell");
                            doc.WriteElementString("Name: ", s);
                            doc.WriteElementString("Content: ", contentsofCell.ToString());
                            doc.WriteEndElement();
                        }
                        else
                        {
                            Formula contentsOfCell = (Formula)content;

                            doc.WriteStartElement("Cell");
                            doc.WriteElementString("Name", s);
                            doc.WriteElementString("Content: =", contentsOfCell.ToString());
                            doc.WriteEndElement();
                        }
                    }

                    doc.WriteEndElement();
                    doc.WriteEndDocument();
                }
            }
            catch (ArgumentException)
            {
                throw new SpreadsheetReadWriteException("Filename was invalid.");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Directory not found.");
            }
            catch (System.IO.FileNotFoundException)
            {
                throw new SpreadsheetReadWriteException("File not found.");
            }

            Changed = false;
        }
    }
}




    