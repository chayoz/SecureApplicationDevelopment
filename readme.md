
# Secure Software Development

Assessment project for the secure software development class


## Installation

Install visual studio community edition(the project was made in the 2019 edition)
Install .net 5.0

```bash
  /git clone <this project>

  Once you open the project, in the Package Manager Console enter:
  add-migration SecureLogin
  update-database
```

The project should be ready to run.
If the users and roles are missing from the database,
go to the Startup.cs file and follow the comments at the Configure() function at the bottom.
