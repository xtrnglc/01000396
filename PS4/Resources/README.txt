"I certify that the work to create this GUI was done entirely by myself and my partner - Trung Le, Adam Sorensen"

=================================================================READ ME FOR PS4=================================================================

Combining PS2 - Dependency Graph, PS3 - Formula Class
Dependency graph uses array of linked lists which might present an efficiency problem in the future. Might need to change but for now we'll stick with it.
Adding DLL from previous PS via resources project
DLLs and XML added succesfully

Created a private class called Cell to represent each cell in a spread sheet.

Note regarding test coverage:
Test coverage at 85.45%
For some reason it also counts lines not being reached in the unit test suite itself as part of the coverage. So lines like } that will not get reached because an exception
is thrown counts as non covered. 
However test suite does cover a majority of the SpreadSheet class library with the only major block not being covered is the addition getValue() getter function from the cell class

All test cases pass although I am unsure how to test what setCellContents returns. It is supposed to return a Iset<string>. Stepping through and debugging seems like the getCellsToRecalculate may have confused
dependents with dependees.

For example the following code:
SpreadSheet s = new SpreadSheet();
s.SetCellContents("A1", 10);
s.SetCellContents("B1", new Formula("A1*2"));
s.SetCellContents("C1", new Formula("B1+A1"));
s.SetCellContents("A1", 12.0);

The dependency graph says that 
A1 is dependent on B1 and C1
B1 is dependent on C1
C1 is dependent on nothing

A1 has no dependees
B1 has dependees A1
C1 has dependees A1 and B1

=================================================================READ ME FOR PS5=================================================================

Regarding a cell formula that contains cells/variables that have not yet been defined, the undefined variable will be set to equal 0 until it has been properly defined
Example:
s.SetContentsOfCell("A1", "10");
s.SetContentsOfCell("B1", "=A1+10");
s.SetContentsOfCell("E1", "=F1+2");

B1 value is 20
E1 value is 2

Regarding a cell formula that contains cells/variables that have been updated, when a cell has been updated with a double, the setContentsOfCell method
gets all other cells that depend on that one cell and reevaluates it.

Still unsure on how to pass in a lookup method delegate for formula evaluate

=================================================================READ ME FOR PS6=================================================================

Got the spreadsheet GUI to work. However unsure about the Invoke() method discussed in class and Worker Thread. We got this implementation working without use of either.
http://www.learning2.eng.utah.edu/mod/page/view.php?id=15705

Our unique features come in the form of the math sub menu which can be used to calculate various methods such as Sum, Average and finding the max/min of a range of cells.
We also have a find and replace method that does as its name suggests.
Tried creating an installer using Visual Studio Installer Project and managed to get it working on my local machine but can't commit to github. When pulled from github the project 
gets corrupted. 

Also in getting the spreadsheet GUI to work how we want it, we modified PS5's spreadsheet which meant that some grading tests for PS5 failed.

***Unique features in the spreadsheet*** - We added the ability to call a handfull of math functions on multiple cells. We also have a find and replace option.
To learn how to use these features, see the help menu.
