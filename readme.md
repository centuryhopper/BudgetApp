### dotnet ef dbcontext scaffold "Name=ConnectionStrings:YourDB" Npgsql.EntityFrameworkCore.PostgreSQL -o Entities -c NameOfYourDbContext --context-dir Contexts -f

### to test api in default profile (http):

    - run "dotnet watch run"

### to test api in https profile:

    - run "dotnet watch run --launch-profile https"

### You need a random 256-bit size string for your jwt:key and you can generate via chatgpt

