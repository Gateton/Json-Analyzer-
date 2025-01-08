const fs = require('fs');
const path = require('path');
const readline = require('readline');
const { stringify } = require('csv-stringify/sync');
const convert = require('xml-js');

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

function analyzeJson(jsonContent) {
  try {
    const parsedJson = JSON.parse(jsonContent);
    console.log("JSON is valid.");

    const stats = analyzeJsonContent(parsedJson);
    console.log(`Unique keys: ${stats.uniqueKeys}`);
    console.log(`Arrays: ${stats.arrays}`);
    console.log(`Nested objects: ${stats.nestedObjects}`);
    console.log(`Primitive values: ${stats.primitiveValues}`);
  } catch (ex) {
    console.log(`JSON is invalid. Error: ${ex.message}`);
  }
}

function analyzeJsonContent(token, stats = { uniqueKeys: 0, arrays: 0, nestedObjects: 0, primitiveValues: 0 }) {
  if (Array.isArray(token)) {
    stats.arrays++;
    token.forEach(item => analyzeJsonContent(item, stats));
  } else if (typeof token === 'object' && token !== null) {
    stats.nestedObjects++;
    stats.uniqueKeys += Object.keys(token).length;
    Object.values(token).forEach(value => analyzeJsonContent(value, stats));
  } else {
    stats.primitiveValues++;
  }
  return stats;
}

function convertJson(jsonContent) {
  rl.question("Select output format:\n1. CSV\n2. XML\nEnter your choice (1 or 2): ", (choice) => {
    let outputPath = '';
    switch (choice) {
      case '1':
        outputPath = 'output.csv';
        convertToCsv(jsonContent, outputPath);
        break;
      case '2':
        outputPath = 'output.xml';
        convertToXml(jsonContent, outputPath);
        break;
      default:
        console.log("Invalid choice. Conversion cancelled.");
        return;
    }
    console.log(`Conversion complete. Output saved to ${outputPath}`);
    rl.close();
  });
}

function convertToCsv(jsonContent, outputPath) {
  const jsonObject = JSON.parse(jsonContent);
  const csv = stringify(jsonObject, { header: true });
  fs.writeFileSync(outputPath, csv);
}

function convertToXml(jsonContent, outputPath) {
  const jsonObject = JSON.parse(jsonContent);
  const xml = convert.json2xml(JSON.stringify(jsonObject), { compact: true, spaces: 2 });
  fs.writeFileSync(outputPath, xml);
}

function main() {
  console.log("JSON Analyzer Tool");
  console.log("------------------");

  function processFile() {
    rl.question("Enter the path to the JSON file (or 'exit' to quit): ", (filePath) => {
      if (filePath.toLowerCase() === 'exit') {
        rl.close();
        return;
      }

      if (!fs.existsSync(filePath)) {
        console.log("File not found. Please try again.");
        processFile();
        return;
      }

      const jsonContent = fs.readFileSync(filePath, 'utf8');
      analyzeJson(jsonContent);

      rl.question("\nDo you want to convert the JSON to another format? (y/n) ", (answer) => {
        if (answer.toLowerCase() === 'y') {
          convertJson(jsonContent);
        } else {
          processFile();
        }
      });
    });
  }

  processFile();
}

main();
