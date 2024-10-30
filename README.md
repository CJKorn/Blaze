# DotNet-Assignment-2
## Instructions
### Host Setup
1. **Open Firewall Port**: On the host machine, open port `9696` in Windows Firewall to allow inbound connections:
   - Go to **Control Panel > System and Security > Windows Defender Firewall > Advanced Settings**.
   - Under **Inbound Rules**, add a new rule to open **TCP Port 9696**.
2. **Get IP Address**: 
   - Open the application, navigate to the **Settings** page.
   - Locate the **Host IP Address** and copy it. This IP will be used by users to connect.

### User Setup
1. **Enter Host IP Address**: 
   - Open the application and navigate to the **Settings** page.
   - Paste the host IP address provided by the host into the designated field to enable the connection.

---

## Location of Assignment Objectives
1. **At least two examples of Interface**: 
- `IHTTPServer.cs` -> `EOIHTTPServer.cs`
- `IReadWrite.cs` -> `JsonReadWrite.cs`
2. **At least one example of NUnit tests**: 

    `JsonReadWriteTests.cs`

3. **At least one example of Anonymous method with LINQ using Lambda expression**:

    `EOIHTTPServer`: Lines 47-49

4. **At least one example of Generics/Generic based Collection**:

    All of `JsonReadWrite.cs`