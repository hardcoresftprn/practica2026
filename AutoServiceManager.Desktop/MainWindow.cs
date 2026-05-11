using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;

namespace AutoServiceManager.Desktop;

public sealed class MainWindow : Window
{
    private readonly DataStore _store = new();
    private UserAccount? _currentUser;
    private string _activePage = "Dashboard";
    private Grid? _shell;
    private StackPanel? _nav;
    private ContentControl? _content;
    private TextBlock? _title;

    private IBrush _sidebar = Brush.Parse("#101828");
    private IBrush _primary = Brush.Parse("#191b23");
    private IBrush _accent = Brush.Parse("#2563eb");
    private IBrush _muted = Brush.Parse("#434655");
    private IBrush _surface = Brushes.White;
    private IBrush _surfaceSoft = Brush.Parse("#f3f3fe");
    private IBrush _background = Brush.Parse("#faf8ff");
    private IBrush _border = Brush.Parse("#c3c6d7");
    private IBrush _sidebarText = Brush.Parse("#dbe2f9");
    private IBrush _cardShadow = Brush.Parse("#14000000");
    private IBrush _success = Brush.Parse("#059669");
    private IBrush _warning = Brush.Parse("#d97706");
    private IBrush _danger = Brush.Parse("#dc2626");
    private IBrush _primarySoft = Brush.Parse("#dbe1ff");

    public MainWindow()
    {
        Title = "AutoService Manager";
        Width = 1280;
        Height = 820;
        MinWidth = 1040;
        MinHeight = 680;
        ApplyThemeColors();
        Background = _background;
        ShowLogin();
    }

    private void ApplyThemeColors()
    {
        if (_store.Data.DarkTheme)
        {
            _sidebar = Brush.Parse("#0b0f19");
            _primary = Brush.Parse("#f0f0fb");
            _accent = Brush.Parse("#2563eb");
            _muted = Brush.Parse("#c3c6d7");
            _surface = Brush.Parse("#1d222e");
            _surfaceSoft = Brush.Parse("#282c3a");
            _background = Brush.Parse("#101828");
            _border = Brush.Parse("#434655");
            _sidebarText = Brush.Parse("#c3c6d7");
            _cardShadow = Brush.Parse("#66000000");
            _success = Brush.Parse("#34d399");
            _warning = Brush.Parse("#fbbf24");
            _danger = Brush.Parse("#f87171");
            _primarySoft = Brush.Parse("#1e3a8a");
        }
        else
        {
            _sidebar = Brush.Parse("#f3f3fe");
            _primary = Brush.Parse("#191b23");
            _accent = Brush.Parse("#2563eb");
            _muted = Brush.Parse("#434655");
            _surface = Brushes.White;
            _surfaceSoft = Brush.Parse("#f3f3fe");
            _background = Brush.Parse("#faf8ff");
            _border = Brush.Parse("#c3c6d7");
            _sidebarText = Brush.Parse("#434655");
            _cardShadow = Brush.Parse("#14000000");
            _success = Brush.Parse("#059669");
            _warning = Brush.Parse("#d97706");
            _danger = Brush.Parse("#dc2626");
            _primarySoft = Brush.Parse("#dbe1ff");
        }

        Background = _background;
    }

    private void ShowLogin(string message = "")
    {
        var email = Input("Email", "testare@gmail.com");
        var password = Input("Parola", "password");
        password.PasswordChar = '*';
        var info = Text(message, 13, _accent);

        var form = new StackPanel
        {
            Width = 440,
            Spacing = 0,
            Children =
            {
                Text("Bun venit inapoi", 32, _primary, FontWeight.SemiBold),
                new Border { Height = 6 },
                Text("Introduceti datele de autentificare pentru a accesa platforma sdsAUTO Enterprise.", 16, _muted),
                new Border { Height = 32 },
                SocialButton("G  Continua cu Google", (_, _) =>
                {
                    _currentUser = _store.Data.Users.FirstOrDefault(u => u.Role == UserRoles.Administrator) ?? _store.Data.Users.First();
                    ShowShell();
                }),
                new Border { Height = 8 },
                SocialButton("M  Continua cu Microsoft SSO", (_, _) =>
                {
                    _currentUser = _store.Data.Users.FirstOrDefault(u => u.Role == UserRoles.User) ?? _store.Data.Users.First();
                    ShowShell();
                }),
                new Border
                {
                    Margin = new Thickness(0, 32, 0, 32),
                    Child = DividerLabel("SAU EMAIL")
                },
                LoginField("Adresa Email Enterprise", email, "mail"),
                new Border { Height = 20 },
                LoginField("Parola", password, "lock", true),
                new CheckBox
                {
                    Content = "Ramaneti conectat pe acest dispozitiv",
                    Margin = new Thickness(0, 20, 0, 24),
                    Foreground = _muted,
                    FontSize = 14
                },
                info,
                PrimaryLoginButton("Autentificare", (_, _) =>
                {
                    var user = _store.Data.Users.FirstOrDefault(u =>
                        u.IsActive &&
                        string.Equals(u.Email, email.Text?.Trim(), StringComparison.OrdinalIgnoreCase) &&
                        u.Password == password.Text);

                    if (user is null)
                    {
                        info.Text = "Email sau parola incorecta.";
                        return;
                    }

                    _currentUser = user;
                    ShowShell();
                }),
                new Border { Height = 36 },
                CenteredText("Nu ai un cont? Contacteaza administratorul flotei", 14, _muted),
                new Border { Height = 26 },
                CenteredText("Termeni si conditii   •   Politica de confidentialitate", 11, Brush.Parse("#737686"))
            }
        };

        var hero = new Grid
        {
            ClipToBounds = true,
            Children =
            {
                AssetImage("login-hero.png", 760, 900),
                new Border
                {
                    Background = new LinearGradientBrush
                    {
                        StartPoint = new RelativePoint(0, 1, RelativeUnit.Relative),
                        EndPoint = new RelativePoint(1, 0, RelativeUnit.Relative),
                        GradientStops =
                        {
                            new GradientStop(Color.Parse("#dd2e3039"), 0),
                            new GradientStop(Color.Parse("#662e3039"), 0.55),
                            new GradientStop(Color.Parse("#002e3039"), 1)
                        }
                    }
                },
                new Grid
                {
                    RowDefinitions = new RowDefinitions("Auto,*,Auto"),
                    Margin = new Thickness(48),
                    Children =
                    {
                        Text("sdsAUTO", 24, Brush.Parse("#b4c5ff"), FontWeight.Black),
                        new StackPanel
                        {
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Spacing = 24,
                            Children =
                            {
                                Text("Viitorul managementului de flote.", 48, Brushes.White, FontWeight.Bold),
                                Text("Sistemul tau integrat pentru monitorizare telematica, analiza de service si optimizarea inventarului in timp real.", 18, Brush.Parse("#ccffffff"))
                            }
                        },
                        new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Spacing = 48,
                            Children =
                            {
                                HeroStat("PERFORMANTA", "99.9%"),
                                HeroStat("SECURITATE", "ISO 27001")
                            }
                        }
                    }
                }
            }
        };
        Grid.SetRow(hero.Children[2], 0);
        Grid.SetRow(((Grid)hero.Children[2]).Children[1], 1);
        Grid.SetRow(((Grid)hero.Children[2]).Children[2], 2);

        var shell = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("1.05*,0.95*"),
            Background = _surface,
            Children =
            {
                hero,
                new Border
                {
                    Background = _surface,
                    Padding = new Thickness(24),
                    Child = new Grid
                    {
                        Children =
                        {
                            new Border
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Child = form
                            }
                        }
                    }
                }
            }
        };
        Grid.SetColumn(shell.Children[1], 1);
        Content = shell;
    }

    private void ShowRegister()
    {
        var name = Input("Nume complet");
        var email = Input("Email");
        var password = Input("Parola");
        password.PasswordChar = '*';
        var role = Select(UserRoles.All, UserRoles.User);
        var info = Text("", 13, _accent);

        Content = Center(Card(new StackPanel
        {
            Width = 440,
            Spacing = 12,
            Children =
            {
                Text("Inregistrare", 26, _primary, FontWeight.Bold),
                Text("Creeaza un utilizator pentru aplicatia desktop.", 13, _muted),
                Label("Nume complet"), name,
                Label("Email"), email,
                Label("Parola"), password,
                Label("Rol"), role,
                info,
                PrimaryButton("Creeaza cont", (_, _) =>
                {
                    if (Blank(name.Text) || Blank(email.Text) || Blank(password.Text))
                    {
                        info.Text = "Completeaza nume, email si parola.";
                        return;
                    }

                    var newName = name.Text?.Trim() ?? "";
                    var newEmail = email.Text?.Trim() ?? "";

                    if (_store.Data.Users.Any(u => string.Equals(u.Email, newEmail, StringComparison.OrdinalIgnoreCase)))
                    {
                        info.Text = "Exista deja un cont cu acest email.";
                        return;
                    }

                    _store.Data.Users.Add(new UserAccount
                    {
                        Id = _store.NextId(_store.Data.Users, u => u.Id),
                        FullName = newName,
                        Email = newEmail,
                        Password = password.Text ?? "",
                        Role = UserRoles.Normalize(role.SelectedItem?.ToString())
                    });
                    _store.Save();
                    ShowLogin("Cont creat. Te poti autentifica.");
                }),
                LinkButton("Inapoi la login", (_, _) => ShowLogin())
            }
        }));
    }

    private void ShowForgotPassword()
    {
        var email = Input("Email");
        var info = Text("", 13, _muted);

        Content = Center(Card(new StackPanel
        {
            Width = 440,
            Spacing = 12,
            Children =
            {
                Text("Recuperare parola", 26, _primary, FontWeight.Bold),
                Text("Pentru proiectul de practica parola demo se afiseaza local.", 13, _muted),
                Label("Email"), email,
                info,
                PrimaryButton("Recupereaza", (_, _) =>
                {
                    var user = _store.Data.Users.FirstOrDefault(u =>
                        string.Equals(u.Email, email.Text?.Trim(), StringComparison.OrdinalIgnoreCase));
                    info.Foreground = user is null ? _accent : _primary;
                    info.Text = user is null
                        ? "Nu exista utilizator cu acest email."
                        : $"Parola pentru {user.FullName}: {user.Password}";
                }),
                LinkButton("Inapoi la login", (_, _) => ShowLogin())
            }
        }));
    }

    private void ShowShell()
    {
        _shell = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("280,*"),
            RowDefinitions = new RowDefinitions("76,*"),
            Background = _background
        };

        _nav = new StackPanel { Background = _sidebar, Spacing = 4 };
        Grid.SetRowSpan(_nav, 2);
        _shell.Children.Add(_nav);

        var topbar = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto"),
            Background = _surface,
            Margin = new Thickness(0, 0, 0, 1)
        };
        Grid.SetColumn(topbar, 1);
        _title = Text(_activePage, 20, _primary, FontWeight.Bold);

        var search = Input("Cauta vehicule, servicii sau clienti...");
        search.Width = 430;
        search.MinWidth = 320;
        search.MaxWidth = 460;
        search.Padding = new Thickness(44, 10, 14, 10);
        search.VerticalAlignment = VerticalAlignment.Center;
        search.HorizontalAlignment = HorizontalAlignment.Left;
        search.Background = _surfaceSoft;
        search.BorderBrush = _border;
        topbar.Children.Add(new Border
        {
            Padding = new Thickness(32, 0, 20, 0),
            VerticalAlignment = VerticalAlignment.Center,
            Child = new Grid
            {
                Children =
                {
                    search,
                    new TextBlock
                    {
                        Text = IconGlyph("search"),
                        FontSize = 16,
                        Foreground = Brush.Parse("#737686"),
                        Margin = new Thickness(16, 0, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                }
            }
        });

        var userBox = new Border
        {
            Background = Brushes.Transparent,
            Padding = new Thickness(0),
            Margin = new Thickness(0, 0, 32, 0),
            VerticalAlignment = VerticalAlignment.Center,
            Child = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 14,
                Children =
                {
                    TopbarIcon("notifications"),
                    TopbarIcon("help"),
                    TopbarIcon("settings"),
                    new Border { Width = 1, Height = 32, Background = _border, Margin = new Thickness(4, 0) },
                    new StackPanel
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        Children =
                        {
                            Text(_currentUser?.FullName ?? "Utilizator", 14, _primary, FontWeight.Bold),
                            Text(_currentUser?.Role ?? UserRoles.User, 11, _muted)
                        }
                    },
                    new Border
                    {
                        Width = 40,
                        Height = 40,
                        Background = _primarySoft,
                        BorderBrush = _accent,
                        BorderThickness = new Thickness(2),
                        CornerRadius = new CornerRadius(20),
                        Child = new TextBlock
                        {
                            Text = UserInitials(_currentUser?.FullName),
                            Foreground = _accent,
                            FontWeight = FontWeight.Bold,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        }
                    }
                }
            }
        };
        Grid.SetColumn(userBox, 1);
        topbar.Children.Add(userBox);
        _shell.Children.Add(topbar);

        _content = new ContentControl();
        Grid.SetColumn(_content, 1);
        Grid.SetRow(_content, 1);
        _shell.Children.Add(_content);

        Content = _shell;
        RebuildNav();
        Navigate("Dashboard");
    }

    private void RebuildNav()
    {
        if (_nav is null) return;
        _nav.Children.Clear();
        _nav.Children.Add(new Border
        {
            Padding = new Thickness(30, 30, 24, 34),
            Child = new StackPanel
            {
                Spacing = 6,
                Children =
                {
                    Text("sdsAUTO", 24, _accent, FontWeight.Black),
                    Text("FLEET MANAGEMENT", 11, _sidebarText, FontWeight.SemiBold)
                }
            }
        });

        foreach (var page in new[] { "Dashboard", "Autovehicule", "Rapoarte", "Piese", "Programari", "Facturi" })
        {
            _nav.Children.Add(NavButton(page));
        }

        _nav.Children.Add(new Border { Height = 1, Background = Brush.Parse("#e1e2ed"), Margin = new Thickness(18, 28, 18, 18) });
        _nav.Children.Add(new Border
        {
            Margin = new Thickness(16, 0, 16, 20),
            Child = PrimaryButton("+ Add Vehicle", async (_, _) => await EditVehicle(new VehicleRecord()))
        });
        _nav.Children.Add(NavButton("Clienti"));
        _nav.Children.Add(NavButton("Setari"));
    }

    private Button NavButton(string page)
    {
        var active = page == _activePage;
        var label = page switch
        {
            "Clienti" => "Clienti",
            "Autovehicule" => "Fleet Status",
            "Programari" => "Telematics",
            "Piese" => "Inventory",
            "Facturi" => "Facturi",
            "Rapoarte" => "Service Analysis",
            "Setari" => "Settings",
            _ => page
        };
        var button = new Button
        {
            Content = label,
            HorizontalContentAlignment = HorizontalAlignment.Left,
            Padding = new Thickness(20, 13),
            Margin = new Thickness(16, 2),
            Background = active ? _accent : Brushes.Transparent,
            Foreground = active ? Brushes.White : _sidebarText,
            BorderThickness = new Thickness(0)
        };
        button.CornerRadius = new CornerRadius(8);
        button.Click += (_, _) => Navigate(page);
        return button;
    }

    private void Navigate(string page)
    {
        _activePage = page;
        if (_title is not null) _title.Text = page;
        RebuildNav();
        if (_content is null) return;

        _content.Content = page switch
        {
            "Dashboard" => DashboardPage(),
            "Clienti" => ClientsPage(),
            "Autovehicule" => VehiclesPage(),
            "Programari" => OrdersPage(),
            "Piese" => PartsPage(),
            "Facturi" => InvoicesPage(),
            "Rapoarte" => ReportsPage(),
            "Setari" => SettingsPage(),
            _ => DashboardPage()
        };
    }

    private Control DashboardPage()
    {
        var activeOrders = _store.Data.Orders.Count(o => o.Status is "Noua" or "In lucru");
        var revenue = _store.Data.Invoices.Where(i => i.Status == "Achitata").Sum(i => i.Total);
        var lowStock = _store.Data.Parts.Count(p => p.Quantity <= p.MinQuantity);
        var completed = _store.Data.Orders.Count(o => o.Status == "Finalizata");
        var plannedToday = _store.Data.Orders.Count(o => o.PlannedAt.Date == DateTime.Today && o.Status != "Anulata");
        var totalOrders = _store.Data.Orders.Count;
        var completionRate = totalOrders == 0 ? 0 : (int)Math.Round((double)completed / totalOrders * 100);

        var metrics = new UniformGrid { Columns = 4, Margin = new Thickness(0, 0, 0, 24) };
        metrics.Children.Add(MetricCard("SERVICII ACTIVE", activeOrders.ToString(), "+12%", "S", _accent, _primarySoft));
        metrics.Children.Add(MetricCard("VENIT TOTAL", Money(revenue), "+5%", "RON", Brush.Parse("#565e71"), Brush.Parse("#dbe2f9")));
        metrics.Children.Add(MetricCard("SARCINI PENDINTE", lowStock.ToString(), "-2%", "!", _danger, Brush.Parse("#ffdbcd")));
        metrics.Children.Add(MetricCard("SATISFACTIE", completed > 0 ? "4.9" : "4.7", "+1%", "*", _accent, _primarySoft));

        var bento = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("2*,1*"),
            Margin = new Thickness(0, 0, 0, 24),
            ColumnSpacing = 24
        };
        bento.Children.Add(ServiceAnalysisCard(activeOrders, completed));

        var sideStack = new StackPanel { Spacing = 24 };
        sideStack.Children.Add(PlanServiceCard());
        sideStack.Children.Add(MaintenanceAlertCard(lowStock));
        Grid.SetColumn(sideStack, 1);
        bento.Children.Add(sideStack);

        return Page(new StackPanel
        {
            Spacing = 0,
            Children =
            {
                new StackPanel
                {
                    Spacing = 4,
                    Margin = new Thickness(0, 0, 0, 24),
                    Children =
                    {
                        Text($"Salut, {_currentUser?.FullName?.Split(' ').FirstOrDefault() ?? "User"}!", 32, _primary, FontWeight.Bold),
                        Text("Iata situatia flotei tale astazi.", 16, _muted)
                    }
                },
                metrics,
                bento,
                ActivityCard()
            }
        });
    }

    private Control ClientsPage()
    {
        var query = Input("Cauta dupa nume, telefon, email");
        var list = new StackPanel { Spacing = 8 };
        void Refresh()
        {
            var q = (query.Text ?? "").Trim();
            var clients = _store.Data.Clients
                .Where(c => Contains(c.FullName, q) || Contains(c.Phone, q) || Contains(c.Email, q))
                .OrderBy(c => c.FullName)
                .ToList();
            list.Children.Clear();
            list.Children.Add(Header("Nume", "Telefon", "Email", "Vehicule", "Actiuni"));
            foreach (var c in clients)
            {
                var vehicles = _store.Data.Vehicles.Count(v => v.ClientId == c.Id).ToString();
                list.Children.Add(Row(new[] { c.FullName, c.Phone, c.Email, vehicles }, ("Editeaza", async () => await EditClient(c)), ("Sterge", () => DeleteClient(c))));
            }
        }
        query.TextChanged += (_, _) => Refresh();
        Refresh();

        return Page(new StackPanel
        {
            Spacing = 16,
            Children =
            {
                Toolbar("Gestionare clienti", "Adauga client", async () => await EditClient(new ClientRecord())),
                query,
                Scroll(list)
            }
        });
    }

    private async Task EditClient(ClientRecord client)
    {
        var isNew = client.Id == 0;
        var name = Input("Nume complet", client.FullName);
        var phone = Input("Telefon", client.Phone);
        var email = Input("Email", client.Email);
        var address = Input("Adresa", client.Address);
        var idnp = Input("CNP/CUI", client.Idnp);

        var ok = await FormDialog(isNew ? "Adauga client" : "Editeaza client",
            new Control[] { Label("Nume complet"), name, Label("Telefon"), phone, Label("Email"), email, Label("Adresa"), address, Label("CNP/CUI"), idnp });
        if (!ok) return;
        if (Blank(name.Text) || Blank(phone.Text)) return;

        client.FullName = name.Text!.Trim();
        client.Phone = phone.Text!.Trim();
        client.Email = email.Text?.Trim() ?? "";
        client.Address = address.Text?.Trim() ?? "";
        client.Idnp = idnp.Text?.Trim() ?? "";
        if (isNew)
        {
            client.Id = _store.NextId(_store.Data.Clients, c => c.Id);
            _store.Data.Clients.Add(client);
        }
        _store.Save();
        Navigate("Clienti");
    }

    private void DeleteClient(ClientRecord client)
    {
        _store.Data.Clients.Remove(client);
        _store.Data.Vehicles.RemoveAll(v => v.ClientId == client.Id);
        _store.Data.Orders.RemoveAll(o => o.ClientId == client.Id);
        _store.Save();
        Navigate("Clienti");
    }

    private Control VehiclesPage()
    {
        var query = Input("Cauta dupa numar, marca, model");
        var list = new StackPanel { Spacing = 8 };
        void Refresh()
        {
            var q = query.Text ?? "";
            list.Children.Clear();
            list.Children.Add(Header("Numar", "Client", "Marca/Model", "Status", "Actiuni"));
            foreach (var v in _store.Data.Vehicles.Where(v => Contains(v.LicensePlate, q) || Contains(v.Brand, q) || Contains(v.Model, q)))
            {
                list.Children.Add(Row(new[]
                {
                    v.LicensePlate,
                    ClientName(v.ClientId),
                    $"{v.Brand} {v.Model} ({v.Year})",
                    v.Status
                }, ("Editeaza", async () => await EditVehicle(v)), ("Sterge", () => DeleteVehicle(v))));
            }
        }
        query.TextChanged += (_, _) => Refresh();
        Refresh();

        return Page(new StackPanel
        {
            Spacing = 16,
            Children =
            {
                Toolbar("Autovehicule", "Adauga autovehicul", async () => await EditVehicle(new VehicleRecord())),
                query,
                Scroll(list)
            }
        });
    }

    private async Task EditVehicle(VehicleRecord vehicle)
    {
        var isNew = vehicle.Id == 0;
        var client = Select(_store.Data.Clients.Select(c => c.FullName).ToArray(), ClientName(vehicle.ClientId));
        var plate = Input("Numar inmatriculare", vehicle.LicensePlate);
        var brand = Input("Marca", vehicle.Brand);
        var model = Input("Model", vehicle.Model);
        var year = Input("An", vehicle.Year == 0 ? "2020" : vehicle.Year.ToString());
        var vin = Input("VIN", vehicle.Vin);
        var mileage = Input("Kilometraj", vehicle.Mileage.ToString());
        var status = Select(new[] { "Activ", "In Service", "Finalizat" }, string.IsNullOrWhiteSpace(vehicle.Status) ? "Activ" : vehicle.Status);

        var ok = await FormDialog(isNew ? "Adauga autovehicul" : "Editeaza autovehicul",
            new Control[] { Label("Client"), client, Label("Numar"), plate, Label("Marca"), brand, Label("Model"), model, Label("An"), year, Label("VIN"), vin, Label("Kilometraj"), mileage, Label("Status"), status });
        if (!ok) return;

        vehicle.ClientId = _store.Data.Clients.FirstOrDefault(c => c.FullName == client.SelectedItem?.ToString())?.Id ?? _store.Data.Clients.First().Id;
        vehicle.LicensePlate = plate.Text?.Trim() ?? "";
        vehicle.Brand = brand.Text?.Trim() ?? "";
        vehicle.Model = model.Text?.Trim() ?? "";
        vehicle.Year = ParseInt(year.Text);
        vehicle.Vin = vin.Text?.Trim() ?? "";
        vehicle.Mileage = ParseInt(mileage.Text);
        vehicle.Status = status.SelectedItem?.ToString() ?? "Activ";
        if (isNew)
        {
            vehicle.Id = _store.NextId(_store.Data.Vehicles, v => v.Id);
            _store.Data.Vehicles.Add(vehicle);
        }
        _store.Save();
        Navigate("Autovehicule");
    }

    private void DeleteVehicle(VehicleRecord vehicle)
    {
        _store.Data.Vehicles.Remove(vehicle);
        _store.Data.Orders.RemoveAll(o => o.VehicleId == vehicle.Id);
        _store.Save();
        Navigate("Autovehicule");
    }

    private Control OrdersPage()
    {
        return Page(new StackPanel
        {
            Spacing = 16,
            Children =
            {
                Toolbar("Programari si comenzi", "Comanda noua", async () => await EditOrder(new ServiceOrderRecord())),
                Scroll(OrdersList(_store.Data.Orders))
            }
        });
    }

    private StackPanel OrdersList(IReadOnlyList<ServiceOrderRecord> orders)
    {
        var list = new StackPanel { Spacing = 8 };
        list.Children.Add(Header("Comanda", "Client", "Vehicul", "Status", "Actiuni"));
        foreach (var o in orders)
        {
            list.Children.Add(Row(new[]
            {
                $"{o.Number} | {o.PlannedAt:dd.MM.yyyy}",
                ClientName(o.ClientId),
                VehicleName(o.VehicleId),
                $"{o.Status} | {Money(OrderTotal(o))}"
            }, ("Editeaza", async () => await EditOrder(o)), ("Anuleaza", () => CancelOrder(o))));
        }
        return list;
    }

    private async Task EditOrder(ServiceOrderRecord order)
    {
        var isNew = order.Id == 0;
        var client = Select(_store.Data.Clients.Select(c => c.FullName).ToArray(), ClientName(order.ClientId));
        var vehicle = Select(_store.Data.Vehicles.Select(VehicleName).ToArray(), VehicleName(order.VehicleId));
        var complaint = Input("Problema reclamata", order.Complaint);
        var diagnosis = Input("Diagnoza", order.Diagnosis);
        var mechanic = Input("Mecanic", order.Mechanic);
        var labor = Input("Manopera", order.LaborCost == 0 ? "0" : order.LaborCost.ToString());
        var status = Select(new[] { "Noua", "In lucru", "Finalizata", "Anulata" }, string.IsNullOrWhiteSpace(order.Status) ? "Noua" : order.Status);

        var ok = await FormDialog(isNew ? "Comanda noua" : "Editeaza comanda",
            new Control[] { Label("Client"), client, Label("Vehicul"), vehicle, Label("Problema"), complaint, Label("Diagnoza"), diagnosis, Label("Mecanic"), mechanic, Label("Manopera"), labor, Label("Status"), status });
        if (!ok) return;

        order.ClientId = _store.Data.Clients.FirstOrDefault(c => c.FullName == client.SelectedItem?.ToString())?.Id ?? _store.Data.Clients.First().Id;
        order.VehicleId = _store.Data.Vehicles.FirstOrDefault(v => VehicleName(v) == vehicle.SelectedItem?.ToString())?.Id ?? _store.Data.Vehicles.First().Id;
        order.Complaint = complaint.Text?.Trim() ?? "";
        order.Diagnosis = diagnosis.Text?.Trim() ?? "";
        order.Mechanic = mechanic.Text?.Trim() ?? "";
        order.LaborCost = ParseMoney(labor.Text);
        order.Status = status.SelectedItem?.ToString() ?? "Noua";
        if (isNew)
        {
            order.Id = _store.NextId(_store.Data.Orders, o => o.Id);
            order.Number = $"CMD-{DateTime.Today:yyyy}-{order.Id:000}";
            order.PlannedAt = DateTime.Today;
            _store.Data.Orders.Add(order);
        }
        _store.Save();
        Navigate("Programari");
    }

    private void CancelOrder(ServiceOrderRecord order)
    {
        order.Status = "Anulata";
        _store.Save();
        Navigate("Programari");
    }

    private Control PartsPage()
    {
        var list = new StackPanel { Spacing = 8 };
        list.Children.Add(Header("Piesa", "Cod", "Furnizor", "Stoc/Pret", "Actiuni"));
        foreach (var p in _store.Data.Parts.OrderBy(p => p.Name))
        {
            var stock = p.Quantity <= p.MinQuantity ? $"ALERTA: {p.Quantity} | {Money(p.Price)}" : $"{p.Quantity} | {Money(p.Price)}";
            list.Children.Add(Row(new[] { p.Name, p.Code, p.Supplier, stock }, ("Editeaza", async () => await EditPart(p)), ("Sterge", () => DeletePart(p))));
        }

        return Page(new StackPanel
        {
            Spacing = 16,
            Children =
            {
                Toolbar("Piese de schimb", "Adauga piesa", async () => await EditPart(new PartRecord())),
                Scroll(list)
            }
        });
    }

    private async Task EditPart(PartRecord part)
    {
        var isNew = part.Id == 0;
        var name = Input("Denumire", part.Name);
        var code = Input("Cod", part.Code);
        var supplier = Input("Furnizor", part.Supplier);
        var qty = Input("Cantitate", part.Quantity.ToString());
        var min = Input("Stoc minim", part.MinQuantity.ToString());
        var price = Input("Pret", part.Price.ToString());
        var ok = await FormDialog(isNew ? "Adauga piesa" : "Editeaza piesa",
            new Control[] { Label("Denumire"), name, Label("Cod"), code, Label("Furnizor"), supplier, Label("Cantitate"), qty, Label("Stoc minim"), min, Label("Pret"), price });
        if (!ok) return;

        part.Name = name.Text?.Trim() ?? "";
        part.Code = code.Text?.Trim() ?? "";
        part.Supplier = supplier.Text?.Trim() ?? "";
        part.Quantity = ParseInt(qty.Text);
        part.MinQuantity = ParseInt(min.Text);
        part.Price = ParseMoney(price.Text);
        if (isNew)
        {
            part.Id = _store.NextId(_store.Data.Parts, p => p.Id);
            _store.Data.Parts.Add(part);
        }
        _store.Save();
        Navigate("Piese");
    }

    private void DeletePart(PartRecord part)
    {
        _store.Data.Parts.Remove(part);
        foreach (var order in _store.Data.Orders)
        {
            order.PartIds.Remove(part.Id);
        }
        _store.Save();
        Navigate("Piese");
    }

    private Control InvoicesPage()
    {
        var list = new StackPanel { Spacing = 8 };
        list.Children.Add(Header("Factura", "Comanda", "Data", "Total/Status", "Actiuni"));
        foreach (var i in _store.Data.Invoices.OrderByDescending(i => i.IssueDate))
        {
            list.Children.Add(Row(new[] { i.Number, OrderNumber(i.OrderId), i.IssueDate.ToString("dd.MM.yyyy"), $"{Money(i.Total)} | {i.Status}" },
                ("Achita", () => PayInvoice(i)), ("Export", async () => await ExportInvoice(i))));
        }

        return Page(new StackPanel
        {
            Spacing = 16,
            Children =
            {
                Toolbar("Facturi", "Genereaza factura", async () => await CreateInvoice()),
                Scroll(list)
            }
        });
    }

    private async Task CreateInvoice()
    {
        var uninvoiced = _store.Data.Orders.Where(o => _store.Data.Invoices.All(i => i.OrderId != o.Id)).ToList();
        if (!uninvoiced.Any())
        {
            await Message("Facturi", "Nu exista comenzi fara factura.");
            return;
        }

        var orderSelect = Select(uninvoiced.Select(o => o.Number).ToArray(), uninvoiced[0].Number);
        var ok = await FormDialog("Genereaza factura", new Control[] { Label("Comanda"), orderSelect });
        if (!ok) return;
        var order = uninvoiced.First(o => o.Number == orderSelect.SelectedItem?.ToString());
        var subtotal = OrderTotal(order);
        var invoice = new InvoiceRecord
        {
            Id = _store.NextId(_store.Data.Invoices, i => i.Id),
            Number = $"FAC-{DateTime.Today:yyyy}-{_store.Data.Invoices.Count + 1:000}",
            OrderId = order.Id,
            IssueDate = DateTime.Today,
            Subtotal = subtotal,
            Vat = decimal.Round(subtotal * 0.20m, 2),
            Total = decimal.Round(subtotal * 1.20m, 2),
            Status = "Neachitata"
        };
        _store.Data.Invoices.Add(invoice);
        _store.Save();
        Navigate("Facturi");
    }

    private void PayInvoice(InvoiceRecord invoice)
    {
        invoice.Status = "Achitata";
        _store.Save();
        Navigate("Facturi");
    }

    private async Task ExportInvoice(InvoiceRecord invoice)
    {
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Export factura",
            SuggestedFileName = $"{invoice.Number}.txt"
        });
        if (file is null) return;
        await using var stream = await file.OpenWriteAsync();
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync($"""
            AutoService Manager
            Factura: {invoice.Number}
            Comanda: {OrderNumber(invoice.OrderId)}
            Data: {invoice.IssueDate:dd.MM.yyyy}
            Subtotal: {Money(invoice.Subtotal)}
            TVA 20%: {Money(invoice.Vat)}
            Total: {Money(invoice.Total)}
            Status: {invoice.Status}
            """);
    }

    private Control ReportsPage()
    {
        var completed = _store.Data.Orders.Count(o => o.Status == "Finalizata");
        var revenue = _store.Data.Invoices.Sum(i => i.Total);
        var paid = _store.Data.Invoices.Where(i => i.Status == "Achitata").Sum(i => i.Total);
        var partsValue = _store.Data.Parts.Sum(p => p.Price * p.Quantity);

        return Page(new StackPanel
        {
            Spacing = 18,
            Children =
            {
                Toolbar("Rapoarte", "Actualizeaza", () => Navigate("Rapoarte")),
                new UniformGrid
                {
                    Columns = 4,
                    Children =
                    {
                        Stat("Comenzi finalizate", completed.ToString(), "Istoric service"),
                        Stat("Venit total", Money(revenue), "Facturi generate"),
                        Stat("Incasari", Money(paid), "Facturi achitate"),
                        Stat("Valoare stoc", Money(partsValue), "Inventar piese")
                    }
                },
                Section("Situatia stocurilor", SimpleRows(_store.Data.Parts.Select(p => $"{p.Name}: {p.Quantity} bucati, valoare {Money(p.Price * p.Quantity)}").ToList())),
                Section("Activitatea service-ului", SimpleRows(_store.Data.Orders.Select(o => $"{o.Number}: {o.Status}, {ClientName(o.ClientId)}, {Money(OrderTotal(o))}").ToList()))
            }
        });
    }

    private Control SettingsPage()
    {
        var darkTheme = new CheckBox
        {
            Content = "Tema intunecata",
            IsChecked = _store.Data.DarkTheme,
            Foreground = _primary,
            FontWeight = FontWeight.SemiBold,
            Margin = new Thickness(0, 4, 0, 0)
        };
        darkTheme.Click += (_, _) =>
        {
            _store.Data.DarkTheme = darkTheme.IsChecked == true;
            _store.Save();
            ApplyThemeColors();
            ShowShell();
            Navigate("Setari");
        };

        return Page(new StackPanel
        {
            Spacing = 18,
            Children =
            {
                Section("Aspect", new StackPanel
                {
                    Spacing = 8,
                    Children =
                    {
                        Text("Alege tema aplicatiei.", 13, _muted),
                        darkTheme
                    }
                }),
                Section("Profil utilizator", SimpleRows(new List<string>
                {
                    $"Nume: {_currentUser?.FullName}",
                    $"Email: {_currentUser?.Email}",
                    $"Rol: {_currentUser?.Role}",
                    $"Fisier date: {_store.DataFilePath}"
                })),
                Section("Securitate", new StackPanel
                {
                    Spacing = 10,
                    Children =
                    {
                        PrimaryButton("Schimba parola", async (_, _) => await ChangePassword()),
                        DangerButton("Iesire din cont", (_, _) => { _currentUser = null; ShowLogin(); })
                    }
                })
            }
        });
    }

    private async Task ChangePassword()
    {
        if (_currentUser is null) return;
        var password = Input("Parola noua");
        password.PasswordChar = '*';
        var ok = await FormDialog("Schimba parola", new Control[] { Label("Parola noua"), password });
        if (!ok || Blank(password.Text)) return;
        _currentUser.Password = password.Text!;
        _store.Save();
        await Message("Setari", "Parola a fost schimbata.");
    }

    private StackPanel LoginField(string label, TextBox input, string icon, bool showForgot = false)
    {
        var header = new Grid { ColumnDefinitions = new ColumnDefinitions("*,Auto") };
        header.Children.Add(Text(label, 13, _muted, FontWeight.SemiBold));
        if (showForgot)
        {
            var forgot = LinkButton("Ai uitat parola?", (_, _) => ShowForgotPassword());
            forgot.Padding = new Thickness(0);
            forgot.Background = Brushes.Transparent;
            Grid.SetColumn(forgot, 1);
            header.Children.Add(forgot);
        }

        input.MinHeight = 46;
        input.Padding = new Thickness(44, 10, showForgot ? 44 : 14, 10);
        input.Background = _surfaceSoft;
        input.BorderBrush = _border;

        return new StackPanel
        {
            Spacing = 7,
            Children =
            {
                header,
                new Grid
                {
                    Children =
                    {
                        input,
                        new TextBlock
                        {
                            Text = IconGlyph(icon),
                            FontSize = 17,
                            Foreground = Brush.Parse("#737686"),
                            Margin = new Thickness(16, 0, 0, 0),
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center
                        },
                        showForgot
                            ? new TextBlock
                            {
                                Text = "show",
                                FontSize = 11,
                                Foreground = Brush.Parse("#737686"),
                                Margin = new Thickness(0, 0, 14, 0),
                                HorizontalAlignment = HorizontalAlignment.Right,
                                VerticalAlignment = VerticalAlignment.Center
                            }
                            : new Border { Width = 0, Height = 0 }
                    }
                }
            }
        };
    }

    private Grid DividerLabel(string label)
    {
        var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("*,Auto,*"), ColumnSpacing = 16 };
        grid.Children.Add(new Border { Height = 1, Background = _border, VerticalAlignment = VerticalAlignment.Center });
        var text = Text(label, 11, Brush.Parse("#737686"), FontWeight.SemiBold);
        Grid.SetColumn(text, 1);
        grid.Children.Add(text);
        var right = new Border { Height = 1, Background = _border, VerticalAlignment = VerticalAlignment.Center };
        Grid.SetColumn(right, 2);
        grid.Children.Add(right);
        return grid;
    }

    private StackPanel HeroStat(string label, string value)
    {
        return new StackPanel
        {
            Spacing = 4,
            Children =
            {
                Text(label, 11, Brush.Parse("#99ffffff"), FontWeight.SemiBold),
                Text(value, 24, Brushes.White, FontWeight.SemiBold)
            }
        };
    }

    private TextBlock CenteredText(string text, double size, IBrush brush)
    {
        var block = Text(text, size, brush);
        block.TextAlignment = TextAlignment.Center;
        block.HorizontalAlignment = HorizontalAlignment.Center;
        return block;
    }

    private Button SocialButton(string text, EventHandler<Avalonia.Interactivity.RoutedEventArgs> click)
    {
        var button = new Button
        {
            Content = text,
            Background = _surface,
            Foreground = _muted,
            BorderBrush = _border,
            BorderThickness = new Thickness(1),
            Padding = new Thickness(18, 12),
            HorizontalContentAlignment = HorizontalAlignment.Center,
            CornerRadius = new CornerRadius(8),
            FontSize = 15
        };
        button.Click += click;
        return button;
    }

    private Button PrimaryLoginButton(string text, EventHandler<Avalonia.Interactivity.RoutedEventArgs> click)
    {
        var button = PrimaryButton(text, click);
        button.MinHeight = 48;
        button.FontWeight = FontWeight.Bold;
        button.HorizontalAlignment = HorizontalAlignment.Stretch;
        return button;
    }

    private Border TopbarIcon(string icon)
    {
        return new Border
        {
            Width = 38,
            Height = 38,
            CornerRadius = new CornerRadius(19),
            Background = Brushes.Transparent,
            Child = new TextBlock
            {
                Text = IconGlyph(icon),
                FontSize = 15,
                Foreground = _muted,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            }
        };
    }

    private static string UserInitials(string? name)
    {
        var parts = (name ?? "User").Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return "US";
        if (parts.Length == 1) return parts[0][..Math.Min(2, parts[0].Length)].ToUpperInvariant();
        return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
    }

    private Control Toolbar(string title, string buttonText, Action action)
    {
        var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("*,Auto") };
        grid.Children.Add(new StackPanel
        {
            Children =
            {
                Text(title, 24, _primary, FontWeight.Bold),
                Text("Operatiile de adaugare, editare, stergere si cautare sunt active.", 13, _muted)
            }
        });
        var button = PrimaryButton(buttonText, (_, _) => action());
        Grid.SetColumn(button, 1);
        grid.Children.Add(button);
        return grid;
    }

    private Border MetricCard(string label, string value, string note, string icon, IBrush accent, IBrush iconBackground)
    {
        var valueSize = value.Length > 10 ? 26 : 38;
        var header = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto") };
        header.Children.Add(new Border
        {
            Width = 44,
            Height = 44,
            Background = iconBackground,
            CornerRadius = new CornerRadius(8),
            Child = Text(icon, icon.Length > 2 ? 11 : 18, accent, FontWeight.Bold)
        });
        var deltaBadge = new Border
        {
            Background = iconBackground,
            CornerRadius = new CornerRadius(6),
            Padding = new Thickness(8, 4),
            Child = Text(note, 11, note.StartsWith('-') ? _danger : _accent, FontWeight.SemiBold)
        };
        Grid.SetColumn(deltaBadge, 2);
        header.Children.Add(deltaBadge);

        return Card(new StackPanel
        {
            Spacing = 14,
            Children =
            {
                header,
                new StackPanel
                {
                    Spacing = 4,
                    Children =
                    {
                        Text(label, 12, _muted, FontWeight.SemiBold),
                        Text(value, valueSize, _primary, FontWeight.Bold)
                    }
                }
            }
        });
    }

    private Border ServiceAnalysisCard(int activeOrders, int completed)
    {
        var title = new Grid { ColumnDefinitions = new ColumnDefinitions("*,Auto") };
        title.Children.Add(new StackPanel
        {
            Spacing = 2,
            Children =
            {
                Text("Analiza Serviciilor", 24, _primary, FontWeight.Bold),
                Text("Performanta saptamanala a atelierului", 13, _muted)
            }
        });
        var range = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
        range.Children.Add(SmallButton("Saptamana", async (_, _) => await Message("Analiza", "Afisare saptamanala activa.")));
        range.Children.Add(SmallButton("Luna", async (_, _) => await Message("Analiza", "Afisarea lunara va agrega toate comenzile din luna curenta.")));
        Grid.SetColumn(range, 1);
        title.Children.Add(range);

        var body = new Grid { ColumnDefinitions = new ColumnDefinitions("1*,1*"), ColumnSpacing = 48 };
        body.Children.Add(new Grid
        {
            Width = 260,
            Height = 260,
            HorizontalAlignment = HorizontalAlignment.Center,
            Children =
            {
                new Border
                {
                    Width = 220,
                    Height = 220,
                    BorderBrush = _surfaceSoft,
                    BorderThickness = new Thickness(20),
                    CornerRadius = new CornerRadius(110),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                },
                new Border
                {
                    Width = 220,
                    Height = 220,
                    BorderBrush = _accent,
                    BorderThickness = new Thickness(20, 20, 20, 0),
                    CornerRadius = new CornerRadius(110),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                },
                new Border
                {
                    Width = 170,
                    Height = 170,
                    BorderBrush = Brush.Parse("#565e71"),
                    BorderThickness = new Thickness(0, 0, 14, 14),
                    CornerRadius = new CornerRadius(85),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Opacity = 0.85
                },
                new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Children =
                    {
                        Text("100%", 32, _primary, FontWeight.Bold),
                        Text("SERVICII", 10, _muted, FontWeight.SemiBold)
                    }
                }
            }
        });

        var legend = new StackPanel { Spacing = 14, VerticalAlignment = VerticalAlignment.Center };
        legend.Children.Add(LegendRow("Reparatii", "45%", _accent));
        legend.Children.Add(LegendRow("Schimb Ulei", "30%", Brush.Parse("#565e71")));
        legend.Children.Add(LegendRow("Diagnoza", "25%", Brush.Parse("#943700")));
        legend.Children.Add(new Border { Height = 1, Background = _border, Margin = new Thickness(0, 12, 0, 8) });
        legend.Children.Add(Text("\"Saptamana aceasta am observat o crestere de 15% a programarilor pentru diagnoza electrica la flota de vehicule hibrid.\"", 13, _muted));
        Grid.SetColumn(legend, 1);
        body.Children.Add(legend);

        return Card(new StackPanel
        {
            Spacing = 24,
            Children = { title, body }
        });
    }

    private Border PlanServiceCard()
    {
        return new Border
        {
            Background = _accent,
            CornerRadius = new CornerRadius(12),
            Padding = new Thickness(26),
            MinHeight = 210,
            BoxShadow = new BoxShadows(new BoxShadow { Blur = 20, OffsetY = 4, Color = Color.Parse("#33004ac6") }),
            Child = new StackPanel
            {
                Spacing = 16,
                Children =
                {
                    Text("Planifica Service", 24, Brushes.White, FontWeight.Bold),
                    Text("Gestioneaza interventiile preventive pentru a maximiza disponibilitatea flotei tale BMW si Audi.", 16, Brushes.White),
                    new Border { Height = 18 },
                    WhiteButton("Programeaza Acum", async (_, _) => await EditOrder(new ServiceOrderRecord()))
                }
            }
        };
    }

    private Border MaintenanceAlertCard(int lowStock)
    {
        return new Border
        {
            Background = Brush.Parse("#e1e2ed"),
            BorderBrush = _border,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(12),
            Padding = new Thickness(24),
            BoxShadow = new BoxShadows(new BoxShadow { Blur = 12, OffsetY = 3, Color = ((ISolidColorBrush)_cardShadow).Color }),
            Child = new StackPanel
            {
                Spacing = 14,
                Children =
                {
                    Text("ALERTA MENTENANTA", 12, _muted, FontWeight.SemiBold),
                    Text(lowStock > 0 ? $"{lowStock} piese necesita completare in stoc." : "5 vehicule necesita revizie in urmatoarele 7 zile.", 16, _primary, FontWeight.SemiBold),
                    LinkButton("Vezi lista completa ->", (_, _) => Navigate(lowStock > 0 ? "Piese" : "Programari"))
                }
            }
        };
    }

    private Border ActivityCard()
    {
        var table = new StackPanel();
        table.Children.Add(ActivityHeader());
        foreach (var order in _store.Data.Orders.Take(5))
        {
            table.Children.Add(ActivityRow(order));
        }
        var headerGrid = new Grid { ColumnDefinitions = new ColumnDefinitions("*,Auto") };
        headerGrid.Children.Add(new StackPanel
        {
            Spacing = 2,
            Children =
            {
                Text("Activitate Recenta", 24, _primary, FontWeight.Bold),
                Text("Ultimele servicii procesate in sistem", 13, _muted)
            }
        });
        var filterButton = SmallButton("Filtru", async (_, _) => await Message("Filtru", "Filtrarea rapida este disponibila in paginile Programari, Clienti si Autovehicule."));
        Grid.SetColumn(filterButton, 1);
        headerGrid.Children.Add(filterButton);

        return Card(new StackPanel
        {
            Spacing = 0,
            Children =
            {
                new Border
                {
                    Padding = new Thickness(8, 0, 8, 18),
                    Child = headerGrid
                },
                table,
                new Border
                {
                    Padding = new Thickness(0, 18, 0, 0),
                    Child = SmallButton("Vezi Toate Inregistrarile", (_, _) => Navigate("Programari"))
                }
            }
        });
    }

    private Border ActivityHeader()
    {
        return new Border
        {
            Background = _surfaceSoft,
            BorderBrush = _border,
            BorderThickness = new Thickness(0, 1, 0, 1),
            Padding = new Thickness(24, 14),
            Child = ActivityGrid(
                Text("VEHICUL", 12, _muted, FontWeight.Bold),
                Text("SERVICIU / INTERVENTIE", 12, _muted, FontWeight.Bold),
                Text("STATUS", 12, _muted, FontWeight.Bold),
                Text("ACTIUNI", 12, _muted, FontWeight.Bold))
        };
    }

    private Border ActivityRow(ServiceOrderRecord order)
    {
        var vehicle = _store.Data.Vehicles.FirstOrDefault(v => v.Id == order.VehicleId);
        var service = string.IsNullOrWhiteSpace(order.Diagnosis) ? order.Complaint : order.Diagnosis;
        var statusBrush = order.Status switch
        {
            "Finalizata" => _success,
            "In lucru" => _warning,
            "Anulata" => _danger,
            _ => _accent
        };

        return new Border
        {
            BorderBrush = _border,
            BorderThickness = new Thickness(0, 0, 0, 1),
            Padding = new Thickness(24, 18),
            Child = ActivityGrid(
                new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Spacing = 12,
                    Children =
                    {
                        AssetImage(VehicleAsset(order.Id), 50, 50),
                        new StackPanel
                        {
                            Spacing = 2,
                            Children =
                            {
                                Text(vehicle is null ? "Vehicul necunoscut" : $"{vehicle.Brand} {vehicle.Model}", 16, _primary, FontWeight.Bold),
                                Text(vehicle?.LicensePlate ?? "Fara numar", 12, _muted, FontWeight.SemiBold)
                            }
                        }
                    }
                },
                new StackPanel
                {
                    Spacing = 2,
                    Children =
                    {
                        Text(service, 15, order.Status == "Anulata" ? _danger : _primary),
                        Text($"Client: {ClientName(order.ClientId)}", 12, _muted)
                    }
                },
                Badge(order.Status.ToUpperInvariant(), statusBrush),
                SmallButton("Vezi", async (_, _) => await EditOrder(order)))
        };
    }

    private Grid ActivityGrid(Control vehicle, Control service, Control status, Control action)
    {
        var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("1.3*,1.6*,.7*,.5*"), ColumnSpacing = 18 };
        grid.Children.Add(vehicle);
        Grid.SetColumn(service, 1);
        grid.Children.Add(service);
        Grid.SetColumn(status, 2);
        grid.Children.Add(status);
        Grid.SetColumn(action, 3);
        grid.Children.Add(action);
        return grid;
    }

    private Border ServiceIcon(VehicleRecord? vehicle)
    {
        var initials = vehicle is null
            ? "?"
            : string.Concat(vehicle.Brand.Take(1)).ToUpperInvariant() + string.Concat(vehicle.Model.Take(1)).ToUpperInvariant();

        return new Border
        {
            Width = 50,
            Height = 50,
            CornerRadius = new CornerRadius(10),
            Background = _primarySoft,
            BorderBrush = _border,
            BorderThickness = new Thickness(1),
            Child = new TextBlock
            {
                Text = string.IsNullOrWhiteSpace(initials) ? "A" : initials,
                FontSize = 16,
                Foreground = _accent,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            }
        };
    }

    private static string VehicleAsset(int orderId)
    {
        return (orderId % 3) switch
        {
            1 => "vehicle-bmw.png",
            2 => "vehicle-tesla.png",
            _ => "vehicle-audi.png"
        };
    }

    private Border LegendRow(string label, string value, IBrush color)
    {
        var dot = new Border
        {
            Width = 10,
            Height = 10,
            CornerRadius = new CornerRadius(5),
            Background = color,
            VerticalAlignment = VerticalAlignment.Center
        };
        var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto"), ColumnSpacing = 12 };
        grid.Children.Add(dot);
        var labelText = Text(label, 15, _primary, FontWeight.SemiBold);
        Grid.SetColumn(labelText, 1);
        grid.Children.Add(labelText);
        var valueText = Text(value, 14, _primary, FontWeight.SemiBold);
        Grid.SetColumn(valueText, 2);
        grid.Children.Add(valueText);
        return new Border { Child = grid };
    }

    private Border ProgressRow(string label, int percent, IBrush color, string detail)
    {
        percent = Math.Clamp(percent, 0, 100);
        var trackWidth = 220d;
        var grid = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,Auto"),
            RowSpacing = 8
        };
        var header = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,Auto")
        };
        header.Children.Add(Text(label, 14, _primary, FontWeight.SemiBold));
        var detailText = new TextBlock
        {
            Text = $"{percent}%  {detail}",
            FontSize = 12,
            Foreground = _muted,
            FontWeight = FontWeight.SemiBold,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        Grid.SetColumn(detailText, 1);
        header.Children.Add(detailText);
        grid.Children.Add(header);

        var track = new Grid
        {
            Width = trackWidth,
            Height = 9,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Children =
            {
                new Border
                {
                    Background = _surfaceSoft,
                    CornerRadius = new CornerRadius(999)
                },
                new Border
                {
                    Width = Math.Max(8, trackWidth * percent / 100d),
                    Background = color,
                    CornerRadius = new CornerRadius(999),
                    HorizontalAlignment = HorizontalAlignment.Left
                }
            }
        };
        Grid.SetRow(track, 1);
        grid.Children.Add(track);
        return new Border { Child = grid };
    }

    private Border Badge(string text, IBrush brush)
    {
        return new Border
        {
            Background = brush,
            CornerRadius = new CornerRadius(999),
            Padding = new Thickness(12, 5),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center,
            Child = Text(text, 11, Brushes.White, FontWeight.Bold)
        };
    }

    private Border Stat(string label, string value, string note)
    {
        return Card(new StackPanel
        {
            Spacing = 10,
            Children =
            {
                Text(label, 11, _muted, FontWeight.SemiBold),
                Text(value, 32, _primary, FontWeight.Bold),
                Text(note, 13, _accent, FontWeight.SemiBold)
            }
        });
    }

    private Border Section(string title, Control content)
    {
        return Card(new StackPanel
        {
            Spacing = 12,
            Children = { Text(title, 18, _primary, FontWeight.SemiBold), content }
        });
    }

    private StackPanel SimpleRows(IReadOnlyList<string> rows)
    {
        var stack = new StackPanel { Spacing = 8 };
        foreach (var row in rows)
        {
            stack.Children.Add(new Border
            {
                Background = _surfaceSoft,
                BorderBrush = _border,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12),
                Child = Text(row, 13, _primary)
            });
        }
        return stack;
    }

    private Border Header(params string[] values)
    {
        return Row(values, Array.Empty<(string, Action)>(), isHeader: true);
    }

    private Border Row(string[] values, params (string Text, Action Action)[] actions)
    {
        return Row(values, actions, false);
    }

    private Border Row(string[] values, (string Text, Action Action)[] actions, bool isHeader)
    {
        var grid = new Grid { ColumnDefinitions = new ColumnDefinitions(string.Join(",", values.Select(_ => "*")) + (actions.Any() ? ",Auto" : "")) };
        for (var i = 0; i < values.Length; i++)
        {
            var text = Text(values[i], isHeader ? 12 : 13, isHeader ? _muted : _primary, isHeader ? FontWeight.Bold : FontWeight.Normal);
            Grid.SetColumn(text, i);
            grid.Children.Add(text);
        }

        if (actions.Any())
        {
            var panel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            foreach (var action in actions)
            {
                panel.Children.Add(SmallButton(action.Text, (_, _) => action.Action()));
            }
            Grid.SetColumn(panel, values.Length);
            grid.Children.Add(panel);
        }

        return new Border
        {
            Background = isHeader ? Brushes.Transparent : _surface,
            BorderBrush = _border,
            BorderThickness = new Thickness(0, 0, 0, 1),
            Padding = new Thickness(12),
            Child = grid
        };
    }

    private Control Page(Control content)
    {
        return new ScrollViewer
        {
            Padding = new Thickness(32),
            Background = _background,
            Content = content
        };
    }

    private ScrollViewer Scroll(Control child)
    {
        return new ScrollViewer { Content = Card(child), VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
    }

    private Control AssetImage(string fileName, double width, double height)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Assets", fileName);
        if (!File.Exists(path))
        {
            return new Border
            {
                Width = width,
                Height = height,
                Background = _surfaceSoft,
                CornerRadius = new CornerRadius(8)
            };
        }

        return new Border
        {
            Width = width,
            Height = height,
            CornerRadius = new CornerRadius(8),
            ClipToBounds = true,
            Background = _surfaceSoft,
            Child = new Image
            {
                Source = new Bitmap(path),
                Stretch = Stretch.UniformToFill,
                Width = width,
                Height = height
            }
        };
    }

    private Border Card(Control child)
    {
        return new Border
        {
            Background = _surface,
            BorderBrush = _border,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(12),
            Padding = new Thickness(24),
            Margin = new Thickness(0, 0, 0, 12),
            BoxShadow = new BoxShadows(new BoxShadow { Blur = 20, OffsetY = 4, Color = ((ISolidColorBrush)_cardShadow).Color }),
            Child = child
        };
    }

    private Control Center(Control child)
    {
        return new Grid
        {
            Background = _background,
            Children =
            {
                new Border
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = child
                }
            }
        };
    }

    private TextBlock Text(string text, double size, IBrush brush, FontWeight weight = FontWeight.Normal)
    {
        return new TextBlock
        {
            Text = text,
            FontSize = size,
            Foreground = brush,
            FontWeight = weight,
            TextWrapping = TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Center
        };
    }

    private TextBlock Label(string text)
    {
        return Text(text, 13, _primary, FontWeight.SemiBold);
    }

    private TextBox Input(string watermark, string value = "")
    {
        return new TextBox
        {
            PlaceholderText = watermark,
            Text = value,
            MinHeight = 38,
            Padding = new Thickness(10),
            Background = _surfaceSoft,
            Foreground = _primary,
            BorderBrush = _border,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(8)
        };
    }

    private ComboBox Select(string[] values, string selected = "")
    {
        var combo = new ComboBox
        {
            ItemsSource = values,
            MinHeight = 38,
            SelectedItem = values.Contains(selected) ? selected : values.FirstOrDefault(),
            Background = _surfaceSoft,
            Foreground = _primary,
            BorderBrush = _border,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(8)
        };
        return combo;
    }

    private Button PrimaryButton(string text, EventHandler<Avalonia.Interactivity.RoutedEventArgs> click)
    {
        var button = new Button
        {
            Content = text,
            Background = _accent,
            Foreground = Brushes.White,
            Padding = new Thickness(18, 10),
            HorizontalContentAlignment = HorizontalAlignment.Center,
            BorderThickness = new Thickness(0),
            CornerRadius = new CornerRadius(8)
        };
        button.Click += click;
        return button;
    }

    private Button DangerButton(string text, EventHandler<Avalonia.Interactivity.RoutedEventArgs> click)
    {
        var button = PrimaryButton(text, click);
        button.Background = _accent;
        return button;
    }

    private Button WhiteButton(string text, EventHandler<Avalonia.Interactivity.RoutedEventArgs> click)
    {
        var button = new Button
        {
            Content = text,
            Background = Brushes.White,
            Foreground = _accent,
            Padding = new Thickness(18, 12),
            HorizontalAlignment = HorizontalAlignment.Left,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            BorderThickness = new Thickness(0),
            CornerRadius = new CornerRadius(8),
            FontWeight = FontWeight.Bold
        };
        button.Click += click;
        return button;
    }

    private Button LinkButton(string text, EventHandler<Avalonia.Interactivity.RoutedEventArgs> click)
    {
        var button = new Button
        {
            Content = text,
            Background = Brushes.Transparent,
            Foreground = _accent,
            BorderThickness = new Thickness(0),
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        button.Click += click;
        return button;
    }

    private Button SmallButton(string text, EventHandler<Avalonia.Interactivity.RoutedEventArgs> click)
    {
        var button = new Button
        {
            Content = text,
            Padding = new Thickness(10, 6),
            Background = _surfaceSoft,
            Foreground = _primary,
            BorderBrush = _border,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(6)
        };
        button.Click += click;
        return button;
    }

    private async Task<bool> FormDialog(string title, IEnumerable<Control> controls)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 460,
            Height = 620,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        var result = false;
        var stack = new StackPanel { Spacing = 10, Margin = new Thickness(22) };
        stack.Children.Add(Text(title, 22, _primary, FontWeight.Bold));
        foreach (var control in controls)
        {
            stack.Children.Add(control);
        }
        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Spacing = 10 };
        buttons.Children.Add(SmallButton("Anuleaza", (_, _) => dialog.Close()));
        buttons.Children.Add(PrimaryButton("Salveaza", (_, _) => { result = true; dialog.Close(); }));
        stack.Children.Add(buttons);
        dialog.Content = new ScrollViewer { Content = stack };
        await dialog.ShowDialog(this);
        return result;
    }

    private async Task Message(string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 380,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel
            {
                Margin = new Thickness(22),
                Spacing = 18,
                Children =
                {
                    Text(title, 20, _primary, FontWeight.Bold),
                    Text(message, 13, _primary),
                    PrimaryButton("OK", (_, _) => { })
                }
            }
        };
        ((Button)((StackPanel)dialog.Content!).Children[2]).Click += (_, _) => dialog.Close();
        await dialog.ShowDialog(this);
    }

    private string ClientName(int id) => _store.Data.Clients.FirstOrDefault(c => c.Id == id)?.FullName ?? "Fara client";
    private string VehicleName(int id) => VehicleName(_store.Data.Vehicles.FirstOrDefault(v => v.Id == id));
    private string VehicleName(VehicleRecord? v) => v is null ? "Fara vehicul" : $"{v.LicensePlate} - {v.Brand} {v.Model}";
    private string OrderNumber(int id) => _store.Data.Orders.FirstOrDefault(o => o.Id == id)?.Number ?? "Fara comanda";
    private decimal OrderTotal(ServiceOrderRecord order) => order.LaborCost + _store.Data.Parts.Where(p => order.PartIds.Contains(p.Id)).Sum(p => p.Price);
    private static string IconGlyph(string name)
    {
        return name switch
        {
            "mail" => "@",
            "lock" => "#",
            "search" => "?",
            "notifications" => "!",
            "settings" => "*",
            "help" => "?",
            "car" => "A",
            "analytics" => "%",
            "inventory" => "I",
            "report" => "R",
            "dashboard" => "D",
            _ => "+"
        };
    }

    private static bool Contains(string value, string query) => string.IsNullOrWhiteSpace(query) || value.Contains(query, StringComparison.OrdinalIgnoreCase);
    private static bool Blank(string? value) => string.IsNullOrWhiteSpace(value);
    private static int ParseInt(string? value) => int.TryParse(value, out var number) ? number : 0;
    private static decimal ParseMoney(string? value) => decimal.TryParse(value?.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var number) ? number : 0;
    private static string Money(decimal value) => $"{value:0.00} RON";
}
