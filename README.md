GymManagmentSystem (solution)
│
├── GymManagmentSystem.APi        # Presentation layer
│   ├── Controllers               # MVC controllers
│   ├── Views                     # Razor views
│   ├── Extentions                # Service registration / startup extensions
│   ├── MembersPhoto              # Uploaded member photos
│   ├── wwwroot                   # Static files
│   └── Program.cs
│
├── GymManagmentSystem.BLL        # Business logic layer
│   ├── Common                    # Result and ResultKind (operation results)
│   ├── Services                  # Service classes and interfaces (incl. attachments)
│   ├── ViewModels                # Account, Analytics, Booking, Membership,
│   │                             # Member, Plan, Session, Trainer
│   └── MappingMapper.cs          # Entity <-> ViewModel mapping
│
└── GymManagmentSystem.DAL        # Data access layer
    ├── Models                    # Member, Trainer, Plan, Membership, Session,
    │                             # Booking, Category, HealthRecord, ApplicationUser, ...
    ├── Repositories              # Repository classes and interfaces
    ├── Migrations                # EF Core migrations
    ├── DataBaseSeeder.cs
    └── IdentityDataSeed.cs
