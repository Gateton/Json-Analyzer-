# JSON Analyzer C#

JSON Analyzer is a command-line tool that allows users to analyze and validate JSON files. It provides functionality to validate JSON syntax, analyze content structure, and convert JSON to other formats like CSV and XML.

## Features

- JSON syntax validation
- Content analysis (counting unique keys, arrays, nested objects, and primitive values)
- Conversion to CSV and XML formats
- Simple and easy-to-use console interface

## Prerequisites

To run this project, you need to have Node.js installed on your system. You can download it from [nodejs.org](https://nodejs.org/).

## Installation

1. Clone this repository:
   ```
   git clone https://github.com/yourusername/json-analyzer-csharp.git
   ```

2. Navigate to the project directory:
   ```
   cd json-analyzer-csharp
   ```

3. Install the required dependencies:
   ```
   npm install
   ```

## Usage

1. Run the application:
   ```
   node index.js
   ```

2. When prompted, enter the path to your JSON file.

3. The tool will validate the JSON and display analysis results.

4. You'll be asked if you want to convert the JSON to another format (CSV or XML).

5. If you choose to convert, select the desired output format.

6. The converted file will be saved in the same directory as the script.

## Example

```
JSON Analyzer Tool
------------------
Enter the path to the JSON file (or 'exit' to quit): test.json
JSON is valid.
Unique keys: 7
Arrays: 1
Nested objects: 1
Primitive values: 7

Do you want to convert the JSON to another format? (y/n) y
Select output format:
1. CSV
2. XML
Enter your choice (1 or 2): 1
Conversion complete. Output saved to output.csv
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.


