
Author: Trung Le
Date: 09/28/2015
PS4 Implementation.

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