# CSV Categorization with OpenAI API

This is a simple test application designed to open a CSV file, process its rows line by line, and use the OpenAI API to determine the category and subcategory for each data row.

---

## Features

- Reads data from a CSV file.
- Sends each row to the OpenAI API for categorization.
- Outputs the category and subcategory for each row.

---

## Setup Instructions

### 1. Prerequisites
- .NET installed on your machine.
- An OpenAI API key.

### 2. Configuration
- Open the `appsettings.json` file.
- Set your OpenAI API key in the `OpenAIKey` field:
  ```json
  {
    "OpenAIKey": "your-api-key-here"
  }
