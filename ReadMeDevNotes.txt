1. Adjusted version of .net core to 2.1.1
2. Added Nuget packages for swashbuckle AKA swagger for better api interface
3. added a reporting type struture that recursively traverses down employees and their directReporters in order to calculate "NumberOfReports" val
4. added a controller and GET endpoint for REporting Structure
5. Added compensation type with data annotations to force salary to be a positive val with at most 2 decimals. post route defaulted to current time
6. added post and get route
7. data persists by writing to a .json file that is loaded into DB context upon appliaction start (overwrites rather than appends)
8. 2 automated test cases written for post and get routes of compensation controller
9. adjusted startup configs as neccessary