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

TODO
Add example of calling API from JS.
