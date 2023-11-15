# Endpoints
## Authentication
### Login
- **Short Description:** Logs the user in
- **URL:** `/auth/login`
- **Method:** `POST`
- **Require Authorization:** `false`
- **Request Type:**
    ```javascript
    {
      "email": string,
      "password": string
    }
    ```
- **Response Type:**
    ```javascript
    {
      "accessToken": string,
      "refreshToken": string,
      "userId": int
    }
    ``` 
- **Sample Request:** `POST /auth/login`
  ```json
  {
    "email": "creator@example.com",
    "password": "Password123!"
  }
  ```
- **Response Codes:**
  - Correct credentials: `200 Ok`
    - **Sample Response:**
      ```json
      {
        "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIyIiwiVXNlckVtYWlsIjoiY3JlYXRvckBleGFtcGxlLmNvbSIsIlVzZXJOYW1lIjoiY3JlYXRvckBleGFtcGxlLmNvbSIsInJvbGUiOiJDUkVBVE9SIiwibmJmIjoxNzAwMDc4MzQ0LCJleHAiOjE3MDAwNzg2NDQsImlhdCI6MTcwMDA3ODM0NH0.Y7BBHBK-7kxoLYiFH5NxodEa7L0uwvu8cFFAqYGi4M0",
        "userId": "2",
        "refreshToken": "8880f50b7ff6496086c253c0d27780f8"
      }
      ```
  - Incorrect credentials: `401 Unauthorized`
    - **Sample Response:** `-`
### Refresh token
- **Short Description:** Get new access token with a refresh token
- **URL:** `/auth/refresh-token`
- **Method:** `POST`
- **Require Authorization:** `true`
- **Authorized Roles:** `any`
- **Request Type:**
    ```javascript
    {
      "refreshToken": string,
      "userId": int
    }
    ```
- **Response Type:**
    ```javascript
    {
      "accessToken": string,
      "refreshToken": string,
      "userId": int
    }
    ``` 
- **Sample Request:** `POST /auth/login`
  ```json
  {
    "refreshToken": "8880f50b7ff6496086c253c0d27780f8"
    "userId": "2"
  }
  ```
- **Response Codes:**
  - Correct credentials: `200 Ok`
    - **Sample Response:**
      ```json
      {
        "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIyIiwiVXNlckVtYWlsIjoiY3JlYXRvckBleGFtcGxlLmNvbSIsIlVzZXJOYW1lIjoiY3JlYXRvckBleGFtcGxlLmNvbSIsInJvbGUiOiJDUkVBVE9SIiwibmJmIjoxNzAwMDc5MzIyLCJleHAiOjE3MDAwNzk2MjIsImlhdCI6MTcwMDA3OTMyMn0.PDi5gDgGKCLWnFlCh2JjpHKZLP3Wmab4kdlLZntFP3o",
        "userId": "2",
        "refreshToken": "b15532c369b84c41b3c71d1908268771"
      }
      ```
  - Incorrect credentials: `401 Unauthorized`
    - **Sample Response:** `-`
## Account Creation
### Register
- **Short Description:** Creates a new account
- **URL:** `/auth/register`
- **Method:** `POST`
- **Require Authorization:** `false`
- **Request Type:**
    ```javascript
    {
      "email": string,
      "password": string,
      "role": Role
    }

    enum Role {
      Creator = 2,
      Consumer = 3
    }
    ```
- **Response Type:**
    ```javascript
    {
      "id": int,
      "username": string
      "normalizedUsername": string
      "email": string
      "normalizedEmail": string
      "emailConfirmed": bool
      "passwordHash": string,
      "isDeleted": bool
      "deletedAt": DateTime?
    }
    ``` 
- **Sample Request:** `POST /auth/register`
  ```json
  {
    "email": "creator4@example.com"
    "password": "Password123!"
    "role": "2"
  }
  ```
- **Response Codes:**
  - User created successfully: `201 Created`
    - **Sample Response:**
      ```json
      {
        "id": 5,
        "username": "creator4@example.com",
        "normalizedUsername": "CREATOR4@EXAMPLE.COM",
        "email": "creator4@example.com",
        "normalizedEmail": "CREATOR4@EXAMPLE.COM",
        "emailConfirmed": false,
        "passwordHash": "0E63C20429D349EEED0C689DB2E47F1661927CFFFB8124DA456276854575367D0DED966F2C00D9D3A162B98DB8915371A880FB079C86E2AF9C04CC751A9BB9174FF0913A71ABD5CBE112EEAEC812709DB749234CF9ADDECE2937F2A1FC0BF2554E3EA3655EBFB712E0598B45C05FBF893084C7AA1ABF1F990603DD1D9B71400A",
        "isDeleted": false,
        "deletedAt": null
      }
      ```
  - User with the email already exists: `409 Conflict`
    - **Sample Response:**
      ```json
      {
        "statusCode": 409,
        "message": "One or more errors occured!",
        "errors": {
            "generalErrors": [
                "User with that email already exists"
            ]
        }
      }
      ```
### Confirm Email
- **Short Description:** Confirms new user's email and enables user login
- **URL:** `/auth/confirmEmail`
- **Method:** `PUT`
- **Require Authorization:** `false`
- **Request Type:**
    ```javascript
    {
      "token": string,
    }
    ```
- **Response Type:** `-`
- **Sample Request:** `POST /auth/confirmEmail`
  ```json
  {
    "token": "U12M6omCamOgqjUYnpxmnUH39bbw6f08eSp_eBrU6-WA5DWHHxnZyvUTLWoZunwyfYET4FB4dSQ02XszNr01sw"
  }
  ```
- **Response Codes:**
  - Email confirmed successfully: `204 No Content`
    - **Sample Response:** `-`
  - Confirmation token is invalid: `404 Not Found`
    - **Sample Response:** `-`
## Account info
### Profile
- **Short Description:** Get current user's info
- **URL:** `/auth/profile`
- **Method:** `GET`
- **Require Authorization:** `true`
- **Authorized Roles:** `any`
- **Request Type:** `-`
- **Response Type:**
  ```javascript
  {
    "id": int,
    "userName": string,
    "email": string,
    "emailConfirmed": bool
  }
  ```
- **Sample Request:** `GET /auth/profile`
- **Response Codes:**
  - User exists: `200 Ok`
    - **Sample Response:**
      ```javascript
      {
        "id": 2,
        "username": "creator@example.com",
        "email": "creator@example.com",
        "emailConfirmed": true
      }
      ```
  - User does not exist: `404 Not Found`
    - **Sample Response:** `-`
## Account Update
### Delete User
- **Short Description:** Queues current user's deletion in 30 days
- **URL:** `/auth/deleteUser`
- **Method:** `DELETE`
- **Require Authorization:** `true`
- **Authorized Roles:** `any`
- **Request Type:** `-`
- **Response Type:** `-`
- **Sample Request:** `DELETE /auth/deleteUser`
- **Response Codes:**
  - User successfully queued for deletion: `204 No Content`
    - **Sample Response:** `-`
  - Attempting to queue last admin for deletion: `403 Forbidden`
    - **Sample Response:** `-`
### Send Email Change Token
- **Short Description:** Send email change token to the user's new email
- **URL:** `/auth/sendEmailChangeToken`
- **Method:** `POST`
- **Require Authorization:** `true`
- **Authorized Roles:** `any`
- **Request Type:**
  ```javascript
  {
    "emailAddress": string
  }
  ```
- **Response Type:** `-`
- **Sample Request:** `POST /auth/sendEmailChangeToken`
  ```javascript
  {
    "emailAddress": "email@example.com"
  }
  ```
- **Response Codes:**
  - User exists: `204 No Content`
    - **Sample Response:** `-`
  - User does not exist: `404 Not Found`
    - **Sample Response:** `-`
### Change Email
- **Short Description:** Changes the users email to the one linked to the token
- **URL:** `/auth/changeEmail`
- **Method:** `PUT`
- **Require Authorization:** `false`
- **Request Type:**
  ```javascript
  {
    "token": string
  }
  ```
- **Response Type:** `-`
- **Sample Request:** `POST /auth/changeEmail`
- **Response Codes:**
  - Email changed successfully: `204 No Content`
    - **Sample Response:** `-`
  - Email change token is invalid: `404 Not Found`
    - **Sample Response:** `-`
### Send Password Reset token
- **Short Description:** Send password reset token to the user's email (Forgot Password feature)
- **URL:** `/auth/sendPasswordResetToken`
- **Method:** `POST`
- **Require Authorization:** `false`
- **Request Type:**
  ```javascript
  {
    "emailAddress": string
  }
  ```
- **Response Type:** `-`
- **Sample Request:** `POST /auth/sendPasswordResetToken`
  ```javascript
  {
    "emailAddress": "email@example.com"
  }
  ```
- **Response Codes:**
  - User exists: `204 No Content`
    - **Sample Response:** `-`
  - User does not exist: `404 Not Found`
    - **Sample Response:** `-`
### Change Password
- **Short Description:** Given Password reset token and new password, changes user's password
- **URL:** `/auth/changePassword`
- **Method:** `PUT`
- **Require Authorization:** `false`
- **Request Type:**
  ```javascript
  {
    "token": string
    "newPassword": string
  }
  ```
- **Response Type:** `-`
- **Sample Request:** `POST /auth/changePassword`
  ```javascript
  {
    "token": "U12M6omCamOgqjUYnpxmnUH39bbw6f08eSp_eBrU6-WA5DWHHxnZyvUTLWoZunwyfYET4FB4dSQ02XszNr01sw",
    "newPassword": "NewPassword123!@"
  }
  ```
- **Response Codes:**
  - User exists: `204 No Content`
    - **Sample Response:** `-`
  - User does not exist: `404 Not Found`
    - **Sample Response:** `-`
### Update Password
- **Short Description:** Given old and new passwords, changes the password
- **URL:** `/auth/updatePassword`
- **Method:** `PUT`
- **Require Authorization:** `true`
- **Authorized Roles:** `any`
- **Request Type:**
  ```javascript
  {
    "oldPassword": string
    "newPassword": string
  }
  ```
- **Response Type:** `-`
- **Sample Request:** `POST /auth/updatePassword`
  ```javascript
  {
    "oldPassword": "OldPassword123$",
    "newPassword": "NewPassword123!@"
  }
  ```
- **Response Codes:**
  - Password updated successfully: `204 No Content`
    - **Sample Response:** `-`
  - Incorrect old password: `401 Not Authorized`
    - **Sample Response:** `-`
### Update Username
- **Short Description:** Changes user's username
- **URL:** `/auth/updateUsername`
- **Method:** `PUT`
- **Require Authorization:** `true`
- **Authorized Roles:** `any`
- **Request Type:**
  ```javascript
  {
    "newUsername": string
  }
  ```
- **Response Type:** `-`
- **Sample Request:** `POST /auth/updatePassword`
  ```javascript
  {
    "newUsername": "VeryGoodUsername"
  }
  ```
- **Response Codes:**
  - Username updated successfully: `204 No Content`
    - **Sample Response:** `-`
  - User does not exist: `404 Not Found`
    - **Sample Response:** `-`
