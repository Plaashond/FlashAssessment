# Setup
- ensure you have the following to run this project
	- Docker
	- DBeaver (For own convenience)
 - cd to the assessment project and run 3
	- ```docker-compose up --force-recreate -V```
	- if a thread gets deadlocked just run the docker compose again.
	- Then open up http://localhost:8080/swagger/index.html once the backend project shows it is running
	- 

- for testing cd to the testing project and run the following 
- ```dotnet test```
- for code coverage run ```dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover```
## TODO:
- [x] intergate swagger and generate docs automatically
- [x] ensure all endpoints are documented properly
- [x] Use Mssql for the database
- [x] Use ORM other than EF
- [x] Add swagger documentation
- [x] Add crud for message
- [x] Add crud for sensitive words
- [x] Add test project
- [x] Add code coverage report
- [x] Add unit tests for dapper code
	- [ ] sensitive word repository
	- [x] message repository
	- [x] message service
	- [ ] Sensitive word service

- [ ] Discuss what can be done to enhance performance
	- [ ] Put messages in a staging table called CustomerMessages and queue all messages without sanatized text to be processed.
	- [ ] Add signalr to update the sender live with messages as they are sanatized.(Give the illusion of performance)
- [ ] Aditional enhancements
	- [x] remove customer link
	- [x] Add audit check on bleep to set if it needs to save or if it is a once-off.
	- [ ] Add case sensitive check
	- [ ] Add authentication to crud functionality
	- [ ] Document README to show setup steps
	- [ ] Add better exception handling(Response messages)
	- [ ] move database setup to startup
	- [ ] add a way of rating a message in severity to show if the message should be flagged
	- [ ] link messages to users
	- [ ] Flag suspiscous messages
- [x] Add microservice to sanatize a string 
- [x] Add Readme instructions for project

