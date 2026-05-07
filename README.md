# Abysalto-TechAssignment-Mid

Below you can find steps how to start you application.

Open project in VS:

1. Json.Development setup: in AbySalto.Mid.WebApi -> appsettings.Development.json -> change property
     "ConnectionStrings/"DefaultConnection" to the connection string of your local DB
   
2. Create tables in DB: in Package manager console set Default project to "AbySalto.Mid.Infrastructure" and type "update-database"
   
3. Start solution: Set "AbySalto.Mid.WebApi" as startup project. Run it with IIS Express  
