Authors: Trung Le and Adam Sorensen
11/11/2015
CS 3500
PS7 - AgCubio


README FOR PS8 AGCUBIO SERVER BELOW HORIZONTAL LINE
README FOR PS9 AGCUBIO DATABASE AND WEB SERVER BELOW

TO START GAME, 
	START SERVER
	START CLIENT
	ENTER PLAYER NAME
	CLICK CONNECT
	RESIZE THE FORM BY DRAGGING THE RIGHT BORDER TO THE RIGHT (MAKING IT BIGGER)
	GAME WILL NOT START OTHERWISE???

	Currently unfinished :(

	Shout out to the TAs, Yash, Sahana, Matthew, Christyn thanks for helping through the struggle
	

TO DO LIST:
-PROPERLY IMPLEMENT SPLIT
-FIX RESIZE BUG
-FIX EXCEPTION WHEN TRYING TO CONNECT TO NO SERVER
-IMPLEMENT VIEWPORT
-IMPLEMENT ENDGAME SCREEN

KNOWN PROBLEMS:
-NEEDS BREAKPOINT IN ORDER TO SPLIT CORRECTLY
-LAGS WHEN SPACE BAR IS PRESSED, SOMETIMES NEW CUBES ARE CREATED


11/11/2015
	Program that creates a client that connects to a server to simulate a game similar to Agario.
	The AgCubio model contains the cube and world classes. So far have not dealt with the world class. 
	We got the client to succesfully connect to the server and to send a player name to the server. When opening the example client we were able to see 
	a cube representing the player name that we created in our client. 
	
	Needed a way to get the string containing data that is sent from the server from the network code to the gui code.
	Did this by using the same callback method for that is used for connect_to_server. 
	First time the callback is called in connected_to_server that callback method sends the player name to the server and prints a message box saying it is connected
	Second time the callback is called then we have the state with the json style string for the player cube
	Tried to deserialize it into a cube object but failed to get it to parse properly. It only parses the name and the mass.
	The DrawCube method also does not work with the new 8 parameter cube class. 

11/12/2015
	Fixed the issues with the json serialize/deserialize not parsing correctly. Added {get, set} for all member variables of the cube class. 
	We were able to send in the player name and then receive back JSON string data from the server. The data string usually sends enough data for at least 2 cube objects.
	Able to cut the data string so that it isolates the first cube with the player name. 
	Able to deserialize the data string into a cube object and send it into the local drawCube method which draws the cube.
	Changed the callback method so now there are three call back methods
	First callback method is called when a succesful connection is made, it will send the server the player's name
	Second callback is called when a cube representing the player is received from the server. It will then draw the cube along with a food cube.
	It will also add the uncompleted json string to the state string builder and send that state into the i_want_more_data network method along with the third callback.
	The third callback will attempt to continuously receive cubes from the server and draw them.
	The third callback currently does not continuuosly update and only a maximum of 6 food cubes can be drawn. 

11/13/15
	Now displaying all the food coming from the server. Also displays the player name, trying to find out how to center it better.
	Problem with the food cubes being drawn was that we were assuming that the 3rd JSON string being sent (substring[2]) was always going
	to be a partial cube. I fixed this by instead of setting substrings[2] always to null, found the last string in the array and appended it 
	to the string builder and handled it. Now it's always going to find all the full strings that can make a cube and deal with them. Going to 
	work on the move aspect and hopefully get it working.

11/15/15
	Trying to get move working. Unsure about the syntax the protocol expects. Trying to make a move request whenever the mouse moves.
	Tried sending variations of Network.Send(currentState.workSocket, "(move, " + ((int)MouseX).ToString() + ", " + ((int)MouseY).ToString() + ")\n");
	Not sure how the server gets the player cube to move. The player cube loc_x and loc_y do not change after sending the move request

11/16/15
	Tried to get OnPaint method working which runs in the background and does not need to be called by any callback method. Similar to the AgCubio function call map posted
	on the forums. However the OnPaint does not work correctly. It only starts drawing when the form is resized. Then it would draw for one second and stop. Resizing
	sometimes redraws but it is inconsistent. It also does not remove where the cube has previously been. It looks like the request to move makes the server send new information
	for the player cube. This is a problem with our implementation because it keeps adding on the same cube but only changes loc_x and loc_y and mass. 
	Very stuck.

11/17/15
	Had to scrap a lot of things. Modified the world class to have two dictionaries that keep track of players and food respectively. Both use ID as keys.
	Modified the onPaint method to have two different foreach loops iterating over the two dictionaries seperately.
	Had to add a third callback method which updates the cubes into the dictionaries.
	There is an small issue with the player cube where the cube is not exactly what is drawn by the onPaint method. There is a small border around the cube close to the top left
	that is part of the cube but is not filled in with color.
	There is also a error where to get onPaint to start drawing and the game to correctly start, user must resize the form by dragging the right border to the right

	THERE ARE INCONSISTENCIES WITH THE SPLIT REQUEST

	Debugging the code and placing a break point on the Form1_KeyDown method which checks for space bar keypress and sends the split request to the server
	However the code does not even reach the method. It does not even check or stop at the break point. 
	To get split working do the following:
	Start server
	Start our client from visual studios and connect
	Start jim's sample client and connect
	Press spacebar to send split request on our client
	Split works correctly on both clients.
	
	or

	Place a breakpoint at Form1_KeyDown method
	anywhere between lines 308 and 314, step through with f10 and the split works fine

	--------------------------------------------------------------------------------------------------------

	Readme for PS8 AgCubio server

	This is AgCubio on hard mode

	TO DO: Fix the connection in the server network to IvP6Any (won't work right now, but will connect to Jim's client)
	TO DO: Viruses
	TO DO: Polish moving?
	
	We have play tested our server on our client and on another team's client and they both work the same however our server does not work with Jim's.
	Also note: to split on our client press s. The space bar does not work for some reason.
	Also disconnects are not handle gracefully 
	

Design decisions
	
	Updates from the server about generating food, updating player locations and dealing with player absorbing are sent every quarter of a second.
	Attrition rate decreases mass by the attrition rate given in the gamestate every 3 seconds.
	The attrition rate is a percentage. So <attrition>10</attrition> means a cube loses 10% of its mass every 3 seconds.
	The server is also able to parse an XML file descring the game world parameters.
	If no gamestate is used, then the default values are used.


	IMPORTANT  IMPORTANT	IMPORTANT  IMPORTANT	IMPORTANT  IMPORTANT	IMPORTANT  IMPORTANT	IMPORTANT  IMPORTANT

	The gamestate file must be a .txt file named "gamestate"
	It must be in 01000396/PS7/Server/bin/Debug
	The user can edit the values between the elements to edit the game state of the world.
	It must be formatted as follows

	<gamestate>
	<width>1000</width>
	<height>1000</height>
	<maxfood>2000</maxfood>
	<topspeed>500</topspeed>
	<attrition>5</attrition>					//5% of mass
	<foodvalue>20</foodvalue>
	<startmass>1000</startmass>
	<minsplitmass>100</minsplitmass>
	<maxsplits>6</maxsplits>
	<numberofvirus>3</numberofvirus>
	<maxsize>15000</maxsize>
	<mergetimer>10</mergetimer>					//this is in seconds
	<virussize>1000</virussize>
	</gamestate>

	



11/23/2015
	To start we went over the MSDN async socket server example and implemented the two functions in the assignment Server_Awaiting_Client_Loop and Accept_a_New_Client
	We were able to get the server and our client that we wrote to contact each other. We were able to send the player name to the server and the server is able to use it
	in a callback method in server.cs to create a cube object and serialize it with Json and send it back to the client. However after sending the initial player cube Json string
	we are having trouble sending the rest of the cubes over.

11/24/2015
	We have a dictionary keeping track of sockets and cubes where each socket correspond to a unique player cube
	We have a dictionary keeping track of a player cube and its destination
	We are able to send over the player cube and the intitial start up cubes from the server to the client.
	Able to have multiple clients connect to the server
	Able to draw players from other client on seperate clients
	Able to move the player cube
	We did this by have a dictionary of sockets and a tuple representing the destination of where the cube wants to go. 
	In the heartbeat method update, we update the location of the player cube so it eventually reaches its destination.
	The speed is proporionate to the mass of the cube, right now speed is mass / 300 but should be able to get it cleaner.
	And any time the current location of the cube is within 5 units of the destiniation, the cube stays still

11/28/2015
	We got the server to be able to accept and parse an XML file describing the game state of the world.
	If no gamestate is used, then the default values are used.
	The gamestate file must be a .txt file named "gamestate"
	It must be in 01000396/PS7/Server/bin/Debug
	The user can edit the values between the elements to edit the game state of the world.
	Also got all cubes to respect the boundaries of the world. The player cube will not move beyond the given boundary.
	Speed fixed to be constant / cube.mass so that it slows down as it gets bigger

	Created unit testing for model class

11/29/2015
	Able to get player cubes being absorbed kinda working. The client we have might be the issue as there are inconsistencies when a player is being eaten.
	The error is because we try to set font size to 0. SO I commented out the font drawings.
	Sometimes it works correctly and other times we get the big red X and following error:

	See the end of this message for details on invoking 
	just-in-time (JIT) debugging instead of this dialog box.

	************** Exception Text **************
	System.ArgumentException: Value of '0' is not valid for 'emSize'. 'emSize' should be greater than 0 and less than or equal to System.Single.MaxValue.
	Parameter name: emSize
	at System.Drawing.Font.Initialize(FontFamily family, Single emSize, FontStyle style, GraphicsUnit unit, Byte gdiCharSet, Boolean gdiVerticalFont)
	at System.Drawing.Font.Initialize(String familyName, Single emSize, FontStyle style, GraphicsUnit unit, Byte gdiCharSet, Boolean gdiVerticalFont)
	at System.Drawing.Font..ctor(String familyName, Single emSize)
	at AgCubioView.Form1.OnPaint(Object sender, PaintEventArgs e) in C:\Users\trungl\Source\Repos\01000396\PS7\AgCubioView\Form1.cs:line 363
	at System.Windows.Forms.Control.OnPaint(PaintEventArgs e)
	at System.Windows.Forms.Form.OnPaint(PaintEventArgs e)
	at System.Windows.Forms.Control.PaintWithErrorHandling(PaintEventArgs e, Int16 layer)
	at System.Windows.Forms.Control.WmPaint(Message& m)
	at System.Windows.Forms.Control.WndProc(Message& m)
	at System.Windows.Forms.ScrollableControl.WndProc(Message& m)
	at System.Windows.Forms.Form.WndProc(Message& m)
	at System.Windows.Forms.Control.ControlNativeWindow.OnMessage(Message& m)
	at System.Windows.Forms.Control.ControlNativeWindow.WndProc(Message& m)
	at System.Windows.Forms.NativeWindow.Callback(IntPtr hWnd, Int32 msg, IntPtr wparam, IntPtr lparam)

	Also added a new parameter: number of viruses.

12/01/2015
	Our server wasn't drawing properly on other people's client because our cube model class uses doubles and so does not parse correctly when JSON 
	deserializes it on other people's server. 
	Change it all to ints and now it draws and works correctly on Kenny's and Michelle's client however still does not work on Jim's.
	Also getting crazy lag. Might be my internet connection though.

12/02/2015
	Fixed the lag by adding more sends.
	Fixed the error we are getting with the red X.
	Able to get the cube to split and merge correctly once.

	Our algorithm for splitting is:
		1. Half the mass of the existing cube
		2. Create a replica cube
		3. Initialize their start positions (needs work, right now we have it so that they start a little bit away from each other)
		4. Create a Rectangle class object to represent the cubes
		5. Store Rectangles in rectangles dictionary and new cubes in w.listofplayers
		6. Initialize splittime on new cubes

	Our algorithm for dealing with split cubes not merging is:
		1. Using the Rectangle class' Intersect method which returns true if a Rectangle object is touching another Rectangle
		2. If false, i.e. not touching then proceed update location as usual
		3. Else, bounce the cube off by sending it a little bit in the opposite direction

	Our algorithm for remerging is:
		1. After a specified time apart,
		2. The cubes with the same split time will combine their masses
		3. A new cube with the combined mass will replace the "main" cube while deleting all other cubes ("main" cube is the cube that is associated with the socket)

12/03/2015
	Ok I will attempt to explain the remerging.

	When a cube splits, it makes a copy of itself with half its mass and the split and the main both have the same split timer.
	So for example say we have original cube with mass = 8.

	If we split, then we now have two cubes both with mass = 4 and a split time of lets say 1.
	If we split again, we now have 4 cubes. Two with mass = 2 and splittime = 1 and two with mass = 2 and splittime = 2
	If we split again, we now have 8 cubes. Two with mass = 1 and splittime = 1 and two with mass = 1 and splittime = 2 and four with mass = 1 and splittime = 3.
	So after a specified time, the two cubes that were split(i.e. splittime = 1) earliest will merge. So now we have 7 cubes. the two cubes with splittime combine to make a cube with mass = 2 and splittime = 0.
	After time, the two cubes at splittime = 2 will merge. So we now have two cubes of mass = 2 and splittime = 0 and 4 cubes of mass = 1 and splittime = 3.
	Then the four cubes at splittime 3 will merge and we have 2 cubes with splittime 0 and mass 2 and one cube that the four cubes merged into that has splittime = 0 and amss = 4.
	Then the two cubes with mass 2 will combine so now we have two cubes with mass = 4. 
	Finally we merge these two and end up with the cube of mass = 8 we had originally assuming no food were eaten.

	The implementation of this is very funky, To merge, you create a new cube and combine the masses and set the reference of an existing cube equal to the new combined cube and erase all references to other partner cube with same splittime. 
	The merge function ideally would send the dead cubes of mass 0 and new combined cube in the merge function but the client did not seem to get the data? So instead it returns a list of cubes that need to die and the caller function deals with sending
	it over to the client.
	I realize this is very ineffecient and obviously does not work out well :( . 

	Also our move when split is buggy because the friendly teamcubes will always attempt to bounce off each other but if somehow they overlap then they both want to go the opposite way and it does not work as we want it to.
	
	Fixed the issue of eating split cubes. If the "main" cube from the split is being eaten then the socket will then get a new main from one of the other splits.
	If a split that is eaten is not main then carry on as usual. Might present issues in remerging.

--------------------------------------------------------------------------------------------------------

	Readme for PS9 AgCubio database and web server

	Database Description:
	PlayersTable1 - General Information about the server, includes player names, game sessions, time alive, time of death, maximum mass, cubes eaten
	PlayersEaten - Organized into three columns, a game session id, a predator and a prey. The predator is the eater cube and the prey is the eaten cube
	RankingTable - A table to keep track of the top 5 mass achieved

	The tables are independent but ideally should be "linked" via player name/ predator. I am unable to do this via MySQL so far but the code invariant will ensure that 
	the entries on each table is consistent with the others.
	
	 
12/07/2015
	Created a static helper class AccessDatabase to help faciliate database manipulation and extraction. Currently has only one useful function: Insert.
	The MySQL database has been modified to contain two tables. One where a player has not eaten any other players and one where they have. 
	The columns for PlayersTable1 (the one for players that have eaten other players) are
	PlayerID, PlayerName, TimeAlive, MaximumMass, CubesEaten, TimeOfDeath, PlayersEaten
	The columns for PlayersTable2 (the one for players that have not eaten other players) are
	PlayerID, PlayerName, TimeAlive, MaximumMass, CubesEaten, TimeOfDeath

	The idea is to update both tables simultaneously. Currently having issues with the foreign key functionality. 
	
	Also modified the cube class to contain number of cubes eaten, maximum mass achieved and a list of other players they have eaten.
	Will probably add other fields to keep track of time of death and time alive.

	Modified the server to increment cubes eaten for each cube and to check for max mass. 
	Everytime mass is updated it checks if the current mass is larger than the max mass achieved

	TO DO: Update database when a player dies
	TO DO: Go through database and check for player id to make sure it is unique
	TO DO: Web server part	
	
12/08/2015
	Re did the database. Now have three tables.
	PlayersTable1 contains all general information about the player and related game session
	PlayersEaten contains the list of cubes that were eaten and who they were eaten by and the game session
	Rankings table contains the top 5 ranks based on maximum mass achieved
	Our rankings table has 5 entries and ranks. The updateRankings method will attempt to sort and update the rankings accordingly.	

12/09/2015
	Got sending html to the browser working. It is all in the AccessDataBase code.
	
	Player link
	<td><a href=\"http://localhost:11100/games?player=Joe\"><div style=\"height:100%;width:100%\">TEXT</div></a></td>

	Session link
	<td><a href=\"http://localhost:11100/eaten?id=35\"><div style=\"height:100%;width:100%\">TEXT</div></a></td>

	
	TO DO: Add hyperlinks 
	TO DO: Clean up disconnects

12/10/15
	Problem with the sockets is with the readcallback. It thinks the socket is still open and will throw an exception. Looks like it's on another thread.
	We might need to make new network code just to handle the readcallback with the web server requests. 

	hyper links work. 
	TO DO: Clean up disconnects so it releases the socket on disconnect to be reused by another command from the browser
	At the moment after a request is received and html is sent to the browser, the browser expects more data. And trying to manually shut the socket down leads to error.
	look into this.

12/11/15
	Can have multiple web server requests per session now. Not exactly sure how I fixed it. I locked the socket that would be disconnected in all the 
	problem areas, most notably the readcallbackWeb and sendWeb methods that would be using the socket. The readcallback would try and use the socket
	after it went to the sendWeb method which disconnects the socket. I also check to see if the socket is even connected before requesting more data.