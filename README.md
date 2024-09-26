# poc-azurestoragefrombrowserwithentraid
A PoC showing how to access Azure Storage accounts directly from a browser using Entra ID auth.

This code was created using Visual Studio 17.11.4, following these steps in order to reduce to a minimum the noise from unrelated concerns:

#1- Created a Blazor WebAssembly Standalone App, selecting "Authentication type" = "Microsoft identity platform":
![image](https://github.com/user-attachments/assets/c3f5f663-5dd7-4874-8a79-b9a930a9fce3)

#2- Followed the wizard to create an App Registration (it's also possible to re-use an existing one). No extra permissions were added.
![image](https://github.com/user-attachments/assets/ea091065-dccc-4c41-8117-7fdcc55ae438)

#3- Replaced my App Registration's ClientId with 00000000-0000-0000-0000-000000000000 and added comment explaining it needs to be replaced with a real ClientId.

#4- Added default .gitignore and .gitattributes, and made a commit to capture the starting point for the changes that are specific to accessing Azure Storage accounts from the browser using Entra ID auth.

#5- Changed the configuration of the Azure Storage account that is going to be accessed:

From the Azure Portal:
  - Found the Storage Account(s)
  - Went to "Resource sharing (CORS)".
  - Added the URL where the Blazor website is published. E.g. https://localhost:1234 and made sure all methods that were going to be used were selected, e.g. GET, PUT, PATCH, POST, OPTIONS, as well as allowed all the headers (**) and exposed all the headers (*):
  - Went to "Access Control (IAM)"
  - Granted "Storage Blob Data Contributor" to my the user I was using for testing (a different role can be chosen as long as it allows the operations that are going to be requested).

![image](https://github.com/user-attachments/assets/0f49907f-afdb-4611-bb12-3822864f770a)

#6- Granted Azure Storage user_impersonation permission to the App Registration

From the Azure Portal:
  - Found the App Registration.
  - Went to "API Permissions" and "Add a permission". Found "Azure Storage" then "user_impersonation".
  - Granted the admin consent for my organisation.

![image](https://github.com/user-attachments/assets/6b4cfa81-6797-41cf-a094-4c7185902f31)

#7- Made all code modifications required to achieve the goal and made a commit.

On the project:
  - Installed the Azure.Identity nuget package (version 1.12.0).
  - Installed the Azure.Storage.Blobs nuget package (version 12.22.0)
  - Added a new class AccessTokenProviderTokenCredential which maps Microsoft.AspNetCore.Components.WebAssembly.Authentication.AccessTokens to Azure.Core.AccessToken.

On the home page:
  - Created a simple textbox + button.
  - Added a dependency on IAccessTokenProvider.
  - Added the code to upload a new blob to the Azure Storage account using Entra ID authorization.

In Program.cs:
  - Configured MSAL.NET to include scopes for Azure Storage.



