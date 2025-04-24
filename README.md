# DES File Protector

DES File Protector is a .NET Framework 4.8 educational application designed to encrypt and decrypt files using the DES (Data Encryption Standard) algorithm. This project provides a secure way to protect sensitive files by encrypting them with a user-provided key and decrypting them when needed.

## Features

- **File Encryption**: Encrypt files securely using the DES algorithm.
- **File Decryption**: Decrypt files back to their original state.
- **Custom Key Support**: Users can provide a 16-character password to generate the encryption key.
- **Random IV Generation**: Ensures secure encryption by generating a random Initialization Vector (IV) for each file.

## How It Works

The application uses the DES algorithm to encrypt and decrypt files. A 16-character password is required, which is processed to generate an 8-byte key. The encryption process also generates a random 8-byte Initialization Vector (IV) to enhance security. The IV is stored in the encrypted file and used during decryption.

### Encryption Process
1. A 16-character password is provided by the user.
2. The password is processed to generate an 8-byte key.
3. A random 8-byte IV is generated.
4. The file is encrypted using the DES algorithm with the generated key and IV.
5. The IV is prepended to the encrypted file for use during decryption.

### Decryption Process
1. The encrypted file is read, and the first 8 bytes (IV) are extracted.
2. The same 16-character password is used to generate the 8-byte key.
3. The file is decrypted using the DES algorithm with the extracted IV and generated key.

## Installation Instructions

1. **Clone the Repository**:
```bash
git clone https://github.com/your-username/DES-file-protector.git
```
3. **Open the Project**:
   - Open the solution in Visual Studio 2022.
4. **Build the Solution**:
   - Build the solution to restore NuGet packages and compile the project.
5. **Run the Application**:
   - Start the application and use the provided interface to encrypt or decrypt files.

## Requirements

- **Visual Studio**
- **.NET Framework 4.8**

## Usage

1. **Encrypt a File**:
   - Select a file to encrypt.
   - Provide a 16-character password.
   - Specify the output file path.
   - Click the "Encrypt" button.

2. **Decrypt a File**:
   - Select an encrypted file.
   - Provide the same 16-character password used during encryption.
   - Specify the output file path.
   - Click the "Decrypt" button.

## Notes

- The password must be exactly 16 characters long.
- Ensure the same password is used for both encryption and decryption.
- The encrypted file includes the IV, so it is essential to keep the file intact for successful decryption.

## License

This project is licensed under the MIT License.

   
