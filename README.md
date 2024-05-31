A*PathFinding algorithm in a hexagonal grid

In this repository you can find a sample code of the A*pathfinding algorithm uisng a hexagonal grid. I mainly use interface to handle the map, cells and pathfinding. There is a simple UI manager that handles how to choose the star point and end point so you can test as many times as you want. Another feature that was added is the walkable parameter meaning if the algorithm can take that space into account to generate the path. The points are selected with the obstacles are all placed using a different mouse button. (This could be improved by enabling the new unity input system so we can use any device later on).

Next improvements ideas:
-Actually draw the path on a different color not just show it in the UI on the right
-Add different parameters to the spaces (maybe some of them have a bigger or less value of crossing it)
-Add the grid size to the UI so anybody can change the grid parameters
-Adjust the camera to the grid size (the ui is currently anchored so that one is fine)

