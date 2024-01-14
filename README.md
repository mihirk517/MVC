# Product Catalog Web App

This an App which can be used as a e-commerce bookstore application where catalogs are visible publically as well as to registered users.
Admins can meanwhile add/remove/edit the catalog as well as edit genres.

## -- Built with ASP.Net Core's MVC Framework
The Application is created as using ASP.NET Core's MVC framework as well as Razor Pages. It uses Entity Framework Core or EFCore as the ORM and SQL Server as the Database.
The application also makes use of ASP. NET Core's **Identity framework** to add **Roles** and **Identities** for Authorization. Thus the App has currently two personas :
 * User 
	The persona which as read-only access and can view the Catalog and Add it to a cart (in later iterations)

 * Admin
	The persona which can create the Catalog via Add/Delete/Remove entries, can create genres or catagories as well as view Catalog

### -- Architecture 

## N - Tier Architecture

The App follows an N-Tier Architecture pattern and thus segregates the functionality in the following parts :

1. MVC Application
	This is the main application which concerns itself with the Web App and the UI.		
    It uses Razor Pages with Bootstrap for the UI.
    Controllers are responsible for each page's functionality and behavior.
    It defines the configuration of the App to the DB as well as the features like Authorization in [Program.cs](../Program.cs)
2. Data Access
	This tier is a Class Library which governs the Data Access to the SQL Database using EFCore, Migrations and other functions. 
    It implements the **Repository Pattern** as well as the **Unit of Work Pattern** for code extensiblity & testability.
3. Model
	This tier is a Class Library which stores infomation related to Data Model which are used by Data Access Layer to create Tables in the DB.
    This tier also stores information related to ViewModels which are consumed by the MVC application

## Videos

The following video shows the application from a user's perspective to view the Catalog and the details of the selection. It also shows the Admin's perspective in which he can add/remove items in the Catalog manipulate genres.

https://github.com/mihirk517/MVC/assets/50024720/a366e458-e999-4d75-80c0-cda5eebf9ae8

This video shows the SQL Database when the items have been changed.

https://github.com/mihirk517/MVC/assets/50024720/dd39da47-9460-44f2-aa70-868f1584cdc0


