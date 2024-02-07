# forma-api-example

Examples and utils for testing public Forma HTTP APIs.

To install dependencies:

Install bun (<https://bun.sh/>)

```bash
bun install
```

# Get an access token

Create an app in APS (<https://aps.autodesk.com/myapps/>) and update the consts at the top of `index.ts` with the values from the it.

```bash
bun run index.ts
```

# Use the access token to talk to an API:

Create an extension in Forma, reference the client_id from the APS app you created, and install the extension in the project with which you want to test.
If you run `index.ts` as is after setting the constants to match the client/extension through which you're testing, it will log the access/refresh token to the terminal and you can just copy it to Bruno/Postman/whatever HTTP client you're using to test.
You can also just call APIs directly from `index.ts` using the access token, ie. just add code at the bottom of the file.

