Authors: Trung Le and Adam Sorensen
11/11/2015
CS 3500
PS7 - AgCubio

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