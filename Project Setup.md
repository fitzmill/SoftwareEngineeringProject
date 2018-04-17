# __Project Setup__

This document details the steps required to set up this project on your machine.
It starts with downloading the code and setting up the database, then moves on
to set up the web and run the project.

Downloading the project
-----------------------
Clone the github repository from [here](https://github.com/fitzmill/SoftwareEngineeringProject "Tuition Assistant Payment Processing on Github").

The repository contains a folder called __DatabaseScripts__ and it contains the
script to create the database and stored procedures. The other folder is called __NelnetProject__ and it contains the C# and javascript
codebase for the project.


Setting up the database
-----------------------
If you don't already have it, download SQL Server Management Studio from
[here](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017 "Download SQL Server Management Studio").
This will allow you to create the database and insert dummy data and stored
procedures.

Once SSMS is installed, open and run the script __ClearAllDataAndInsertDummyData.sql__
in the __DatabaseScripts__ folder. This will create the database and insert
the dummy data to be used while testing the project.

After running the database creation script, you will need to run all of the stored
procedure creation files located in subfolders within the __DatabaseScripts__ folder
by opening them in SSMS and executing them. Once the scripts have run, you should
be able to see them in the SSMS __Object Explorer__ under
`Databases -> NelnetPaymentProcessing -> Programmability -> Stored Procedures`.
If the stored procedures are not immediately visible here, you may have to refresh
your __Object Explorer__ to see newly created Stored Procedures.


Setting up your node.js environment
-----------------------------------
Before running the steps in this section, be sure to install node.js on your machine.
To set up node.js, open **Windows Powershell** or the terminal of your choice and
navigate to your project directory, then to __NelnetProject\Web__. First run
`npm install` to set up node in that directory. Then run `npm run watch` to compile
the UI for the project.


Running the project
-------------------
Now open the solution in **Visual Studio**, right click the **Web** project, and
select it as the startup project. Now you can run the project, which takes a few
seconds to start.

Once the project has started you will be on the login page. From here, you can
log in using the following data that was added through the database creation script.

```
----- General User 1 -----
Email: billy@microsoft.com
Password: ImBill1997$

------ Admin User 1 ------
Email: sean@weebnation.com
Password: Sean2010!
```

Logging in as a general user will allow you to view and edit their information
from their account dashboard. Logging in as an admin will allow you to view all
generated reports as well as generate your own custom reports and view those.

If you would like to create an account, you can navigate to the account creation
page where you will be prompted to enter the necessary information. Since the
payment information must be valid, you can use the following test cards provided
by ***PaymentSpring*** to simulate a real card.

| Card Type | Card Number      | CSC  | Expiration Date |
| --------- | :--------------: | :--: | :-------------: |
| Visa      | 4111111111111111 | 999  | 08/2018         |
| Amex	    | 345829002709133  | 9997 |	08/2018         |
| Discover	| 6011010948700474 | 999  |	08/2018         |
| Mastercard| 5499740000000057 | 999  | 08/2018         |
