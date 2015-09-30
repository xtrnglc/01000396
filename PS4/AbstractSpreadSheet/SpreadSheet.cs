/*
Author: Trung Le
Date: 09/29/2015
Purpose: SpreadSheet class implementation
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;

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
    public class SpreadSheet : AbstractSpreadsheet
    {

        private DependencyGraph dependecies;
        private Dictionary<string, Cell> cellList;



        /// <summary>
        /// Represents a cell
        /// Can contain either a Formula, a string, a FormulaError or a double
        /// </summary>
        private class Cell
        {
            string stringContent = null;
            double doubleContent = double.PositiveInfinity;
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
                doubleContent = content;
            }

            /// <summary>
            /// To represent a cell holding a formula. 
            /// If the formula can be evaluated then assign the result to doubleContent
            /// Else, assign formulaError to formulaErrorContent
            /// </summary>
            /// <param name="content"></param>
            public Cell(Formula content)
            {
                formulaContent = content;

                try
                {
                    doubleContent = (double)content.Evaluate(s => 0);
                }
                catch (InvalidCastException)
                {
                    formulaErrorContent = (FormulaError)content.Evaluate(s => 0);
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
                else if (doubleContent != double.PositiveInfinity)
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
        /// Constructor
        /// </summary>
        public SpreadSheet()
        {
            dependecies = new DependencyGraph();
            cellList = new Dictionary<string, Cell>();
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
            if (string.IsNullOrEmpty(name) | !isVariable(name))
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
        public override ISet<String> SetCellContents(String name, double number)
        {
            if (string.IsNullOrEmpty(name) | !isVariable(name))
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

            if (dependecies.HasDependees(name))
            {
                HashSet<String> cellsToRecalculate = new HashSet<string>();

                foreach (String s in GetCellsToRecalculate(name))
                    cellsToRecalculate.Add(s);

                return cellsToRecalculate;
            }

            return new HashSet<String> { name };

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
        public override ISet<String> SetCellContents(String name, String text)
        {
            if (text == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(name) | !isVariable(name))
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
            

            if (dependecies.HasDependees(name))
            {
                HashSet<String> cellsToRecalculate = new HashSet<string>();

                foreach (String s in GetCellsToRecalculate(name))
                    cellsToRecalculate.Add(s);

                return cellsToRecalculate;
            }

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
        public override ISet<String> SetCellContents(String name, Formula formula)
        {
            if (formula == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(name) | !isVariable(name))
            {
                throw new InvalidNameException();
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

            if (dependecies.HasDependees(name))
            {
                return dependecies.GetDependees(name);
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
        /// PLEASE NOTE THAT THIS METHOD DEPENDS ON THE ABSTRACT METHOD GetDirectDependents.
        /// IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
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
            return Regex.IsMatch(s, "^((_)*[a-zA-Z]+(_)*[1-9][0-9]*)|(_)+$");
        }
    }
}

/*
private void updateDependency(String name)
        {
            try
            {
                Cell temp;
                sp.TryGetValue(name, out temp);
                Formula formula = (Formula)temp.getContents();

                foreach (String s in formula.GetVariables())
                    dg.RemoveDependency(name, s);
            }
            catch (InvalidCastException) { }
        }



    private ISet<String> processLinks(String name, Formula formula, IEnumerable<String> oldDependents)
        {
            dg.ReplaceDependents(name, formula.GetVariables());

            try
            {
                IEnumerable<String> use = GetCellsToRecalculate(name);
                HashSet<String> toReturn = new HashSet<string>();

                foreach (String s in use)
                    toReturn.Add(s);

                return toReturn;
            }
            catch (CircularException)
            {
                foreach (String s in formula.GetVariables())
                    dg.RemoveDependency(name, s);

                throw new CircularException();
            }
        }
*/
