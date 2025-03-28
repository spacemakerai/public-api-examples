# forma-api-example

Examples and utils for testing public Forma HTTP APIs.

To install dependencies:

Install bun (<https://bun.sh/docs/installation>).

Then, in this repo:

```bash
bun install
```

# Get an access token

Create an app in APS (<https://aps.autodesk.com/myapps/>). The code in `index.ts`
implements the Authorization Code Grant (PKCE) for public clients flow described
[here](https://aps.autodesk.com/en/docs/oauth/v2/tutorials/get-3-legged-token-pkce/get-3-legged-token-pkce/)
. Make sure you choose "Desktop, Mobile, Single-Page App" for the application
type. Update the const at the top of `index.ts` with the values from the
application you created or set `CLIENT_ID` in a `.env` file.

```bash
bun run index.ts
```

# Use the access token to talk to an API:

Create an extension in Forma, reference the client_id from the APS app you
created, and install the extension in the project with which you want to
test. If you run `index.ts` as is after setting the constants to match the
client/extension through which you're testing, it will log the access/refresh
token to the terminal and you can just copy it to Bruno/Postman/whatever
HTTP client you're using to test. You can also just call APIs directly from
`index.ts` using the access token, ie. just add code at the bottom of the file.

## Example

```ts
fetch(
  "https://developer.api.autodesk.com/forma/terrain/61a7e758-27f8-4a94-bdfa-ad308e5428b8/revisions/1706175717609?authcontext=pro_0fovoakrca",
  {
    method: "GET",
    headers: {
      accept: "application/json",
      authorization: `Bearer ${tokenResponse.access_token}`,
      "x-ads-region": "EMEA", // US for US region.
    },
  },
)
  .then((res) => {
    if (!res.ok) {
      console.error(res.status, res.statusText);
    }
    return res.json();
  })
  .then((body) => {
    console.log(body);
  });
```
