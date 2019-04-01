## Zeww
The data access layer, busniess logic layer, models, and APIs for Zeww. The backend was designed to support the MVC framework design pattern with a code first approach using entity framework.

## Dependencies

-Microsoft SQL
-Entity Framework
-ASP.Net Core

## About Zeww

Zeww is a communication tool for big teams and groups. Zeww enables its users to create a workspace, invite members, and create internal  channel topics within the workspace. In short, Zeww is a Slack wanna-be.

Zeww is a collabrative project developed by a group of junior developers to practice the following technologies; ASP.Net Core, Entity Framework, SQLExpress, ReactJS, and SignalR. The main objective of the project is to build a working product in a pre-defined timeframe following the Scrum methodology to manage the development process, MVC design pattern, and TFS for version control. 

## Zeww Features

- Sign up
- Sign in
- Create a new workspace
- Add workspace icon
- Join a pre-existing workspace
- Invite other members to workspace
- Direct message other members of workspace
- Create channels within workspace
- Upload/Download files sent over a channel
- Omnisearch within a workspace
- View other workspace member status (Online, offline, busy, etc.)
- Like, Edit, Delete messages.

## Final proudct
 
 Zeww Front-end (https://github.com/Linasaurs/Zeww-FrontEnd)</br>
 
## Running the component

`set Zeww as startup project`</br>
`in the package manager console write the following command: "add-migration init" followed by "update-database"`
`set Zeww.BussniessLogic as startup project`
`run the solution`\

`Note: if there is a problem intiating the database make sure you are using the correct connection string in the appsettings.json file`


This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).
