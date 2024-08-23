# HarmoniX - WPF Music Application

HarmoniX is a WPF-based music application that allows users to upload and listen to music, create playlists, and share them with others. All music files are stored in AWS S3 Buckets.

## Features

- **Upload Music**: Upload MP3 files to AWS S3 Buckets.
- **Create Playlists**: Organize your favorite songs into playlists.
- **Listen to Music**: Stream music directly from AWS S3.
- **Share Playlists**: Share your curated playlists with other users.
- **User Accounts**: Secure user authentication and account management.

## Project Structure

- **HarmoniX-View**: WPF Application for the user interface.
- **HarmoniX-Service**: Services for business logic, including music upload, playlist management, and user authentication.
- **HarmoniX-Repository**: Data access layer to interact with MySQL database.
- **HarmoniX-Controller**: Handles interactions with AWS S3 for uploading and retrieving music files.

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- MySQL Database
- AWS S3 Bucket and Credentials
- Visual Studio 2022

### Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/HarmoniX.git
   cd HarmoniX
## 2. Set Up the Database

- Configure your MySQL database.
- Update the connection string in the `appsettings.json` file of the `HarmoniX-Repository` project.

## 3. Configure AWS S3

- Update the AWS credentials in the `s3bucketkey.json` file of the `HarmoniX-Controller` project.
- Ensure that your AWS S3 bucket is set up to accept MP3 file uploads.

## 4. Build and Run the Application

- Open the solution in Visual Studio.
- Restore NuGet packages.
- Build the solution.
- Run the `HarmoniX-View` project.

## Usage

- **Register/Login**: Create a new account or log in with your existing credentials.
- **Upload Music**: Navigate to the "Upload" section and select your MP3 file. Once uploaded, the file is stored in the AWS S3 Bucket.
- **Create a Playlist**: Create a new playlist by providing a name and description. Add songs to your playlist from your uploaded collection.
- **Listen to Music**: Select a playlist and start streaming your music. Use the queue to manage your listening experience.

## Contributing

We welcome contributions to the HarmoniX project. If you would like to contribute, please follow these steps:

1. Fork the repository.
2. Create a new feature branch.
3. Commit your changes.
4. Push the branch to your forked repository.
5. Submit a pull request.

## License

This project is licensed under the MIT License. See the LICENSE file for details.

## Contact

For any questions or support, please contact:

- **Email**: Quanghuy01062004@gmail.com
- **GitHub**: [[https://github.com/HyuDeQueue/HarmoniX](https://github.com/HyuDeQueue/HarmoniX)]
