# IoT_Assignments ( for junior frontend/backend dev)
 
# Backend
This was the first time I wrote C# specifically.
-------------------------------------------
This is the folder with the projects for the Backend Assignment
IoT_Backend_Assignment: Is the DLL that when built creates the .dll file we will use in the other project to call the IpStack API
WebApi: This is the project that contains part 2 and 3 of the assignent. Contains the classes and methods for the custom API, cache and database handling.

APIs:

get "http://localhost:5137/WebApi/GetIpDetails/-ip-" where -ip- we put the ip we want the details for.

post "http://localhost:5137/WebApi/update" where in the body of the request we put our field to be changed, the new value and the ip that is for ie:

{

  "IP": "127.0.0.1",
  
  "field": "Country",
  
  "value": "Italy"
  
}

get "http://localhost:5137/WebApi/progress/-batch-id-" where -batch-id- is the id of the batch we want to learn its progress

For the 3rd part of the assignment, to make it easier to check if it works etc., it uses batches of 2 requests per batch and waits, at the start, for the requests to be 6 before it start executing them all. That functionality is controlled by 2 variables: 'thresh' and 'batchSize' where 'thresh' is the threshold of number of batches that have to be created before it starts executing requests of each batch and 'batchSize' is the number of requests each batch holds.


# Frontend
-------------------------------------------
This is the folder with the project for the Frontend Assignment

This is a simple web application that retrieves job listings from Norway and displays the most important info in the listing.

The app makes a call to the API to retrieve the data, then it parses the data and keeps the most important of them using an interface. Then the data is displayed in a table across 50+ pages. 

It's not perfect as many things could be added to make it better (more options, more functionalities etc) but it's what I could do with the time (outside of work) that I had.

-------------------------------------------

*Chop wood, carry water*
