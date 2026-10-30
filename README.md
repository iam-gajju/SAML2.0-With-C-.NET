# SAML 2.0 Integration with ASP.NET MVC

This project demonstrates how to integrate SAML 2.0 authentication into an ASP.NET MVC application using the OWIN middleware. It provides Single Sign-On (SSO) functionality, allowing users to authenticate via a SAML Identity Provider (IdP).

## Features

- **SAML 2.0 Authentication**: Securely authenticate users via a SAML IdP.
- **Claims-Based Authentication**: Extract user claims such as email, first name, and last name.
- **Session Management**: Handle user login and logout securely.
- **Error Handling**: Display user-friendly error messages for login failures.

## Prerequisites

- **.NET Framework**: 4.7.2
- **C# Version**: 7.3
- **Visual Studio**: 2022 or later
- **Identity Provider (IdP)**: A configured SAML IdP with metadata.

## Project Structure

- **Controllers**:
  - `AccountController`: Handles login, logout, and user session management.
  - `HomeController`: Provides basic views for the application.
- **Views**:
  - `Account/SignedIn.cshtml`: Displays user claims after successful login.
  - `Account/LoginFailure.cshtml`: Displays an error message for login failures.
  - `Shared/_Layout.cshtml`: Shared layout for the application.
- **Configuration**:
  - `Web.config`: Application configuration.
  - `App_Start/Startup.Auth.cs`: Configures OWIN authentication middleware.
  - `App_Data/idp-metadata.xml`: Stores the IdP metadata.

## Setup Instructions

1. **Clone the Repository**:
