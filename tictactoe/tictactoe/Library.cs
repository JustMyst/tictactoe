using System.Threading.Tasks;
using Xamarin.Forms;
using System;

public class Library
{
    private const string app_title = "Tiktaktoł";
    private const char blank = ' ';
    private const char nought = 'O';
    private const char cross = 'X';
    private const int size = 3;
    private int WhoStarts = 2;
    private int Level = 0;
    private bool single = true;
    private Grid _grid;
    private ContentPage _page;
    public bool _won = false;
    public bool _drawn = false;
    private char _piece = blank;
    private char[,] _board = new char[size, size];
    private string[] StatsL0 = new string[3]{ "0", "0", "0" }, 
        StatsL1 = new string[3] { "0", "0", "0" }, 
        StatsL2 = new string[3] { "0", "0", "0" };


    public Library()
    {
        if (Application.Current.Properties.ContainsKey("StatsL0"))
        {
            StatsL0 = Application.Current.Properties["StatsL0"].ToString().Split(',');
        }
        if (Application.Current.Properties.ContainsKey("StatsL1"))
        {
            StatsL1 = Application.Current.Properties["StatsL1"].ToString().Split(',');
        }
        if (Application.Current.Properties.ContainsKey("StatsL2"))
        {
            StatsL2 = Application.Current.Properties["StatsL2"].ToString().Split(',');
        }

        if (Application.Current.Properties.ContainsKey("Level"))
        {
            Level = Int32.Parse(Application.Current.Properties["Level"].ToString());
        }
        if (Application.Current.Properties.ContainsKey("Single"))
        {
            single = Convert.ToBoolean(Application.Current.Properties["Single"]);
        }
        if (Application.Current.Properties.ContainsKey("WhoStarts"))
        {
            WhoStarts = Int32.Parse(Application.Current.Properties["WhoStarts"].ToString());
        }
    }

    public void Show(string content, string title)
    {
        Device.BeginInvokeOnMainThread(() => {
            _page.DisplayAlert(title, content, "Ok");
        });
    }

    private async Task<bool> ConfirmAsync(string content,
        string title, string ok, string cancel)
    {
        return await _page.DisplayAlert(title, content, ok, cancel);
    }

    private bool Winner()
    {
        return
        (_board[0, 0] == _piece && _board[0, 1] == _piece && _board[0, 2] == _piece) ||
        (_board[1, 0] == _piece && _board[1, 1] == _piece && _board[1, 2] == _piece) ||
        (_board[2, 0] == _piece && _board[2, 1] == _piece && _board[2, 2] == _piece) ||
        (_board[0, 0] == _piece && _board[1, 0] == _piece && _board[2, 0] == _piece) ||
        (_board[0, 1] == _piece && _board[1, 1] == _piece && _board[2, 1] == _piece) ||
        (_board[0, 2] == _piece && _board[1, 2] == _piece && _board[2, 2] == _piece) ||
        (_board[0, 0] == _piece && _board[1, 1] == _piece && _board[2, 2] == _piece) ||
        (_board[0, 2] == _piece && _board[1, 1] == _piece && _board[2, 0] == _piece);
    }

    private bool Drawn()
    {
        return
        _board[0, 0] != blank && _board[0, 1] != blank && _board[0, 2] != blank &&
        _board[1, 0] != blank && _board[1, 1] != blank && _board[1, 2] != blank &&
        _board[2, 0] != blank && _board[2, 1] != blank && _board[2, 2] != blank;
    }

    private Grid Piece()
    {
        Grid grid = new Grid()
        {
            HeightRequest = 60,
            WidthRequest = 60,
        };
        if (_piece == cross)
        {
            BoxView line1 = new BoxView()
            {
                Color = Color.Red,
                HeightRequest = 60,
                WidthRequest = 5,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Rotation = 45,
            };
            BoxView line2 = new BoxView()
            {
                Color = Color.Red,
                HeightRequest = 60,
                WidthRequest = 5,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Rotation = 135,
            };
            grid.Children.Add(line1);
            grid.Children.Add(line2);
        }
        else if (_piece == nought)
        {
            EllipseView circle = new EllipseView()
            {
                Color = Color.Blue,
                HeightRequest = 60,
                WidthRequest = 60,
                StrokeWidth = 5,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            grid.Children.Add(circle);
        }
        return grid;
    }

    private void Add(ref Grid grid, int row, int column)
    {
        Grid element = new Grid()
        {
            HeightRequest = 80,
            WidthRequest = 80,
            Margin = new Thickness(10),
            BackgroundColor = Color.WhiteSmoke,
        };
        TapGestureRecognizer tapped = new TapGestureRecognizer();
        tapped.Tapped += (sender, e) =>
        {
            if (!_won)
            {
                element = (Grid)sender;
                if ((element.Children.Count < 1))
                {
                    element.Children.Add(Piece());
                    _board[(int)element.GetValue(Grid.RowProperty),
                    (int)element.GetValue(Grid.ColumnProperty)] = _piece;
                }


                if (Winner())
                {
                    _won = true;
                    Show($"{_piece} wins!", app_title);
                    if (_piece == nought) { SaveStats(0); }
                    else
                    {
                        SaveStats(2);
                    }
                }
                else if (Drawn())
                {
                    _drawn = true;
                    Show("Droł!", app_title);
                    SaveStats(1);
                }
                else
                {
                    _piece = (_piece == cross ? nought : cross);
                    if (single == true) { ComputerMoves(); }
                }
                

            }
            else
            {
                Show("It's already done! Start Again!", app_title);
            }
            
        };
        element.GestureRecognizers.Add(tapped);
        element.SetValue(Grid.ColumnProperty, column);
        element.SetValue(Grid.RowProperty, row);
        grid.Children.Add(element);
    }

    private void Layout(ref Grid grid)
    {
        grid.Children.Clear();
        grid.ColumnDefinitions.Clear();
        grid.RowDefinitions.Clear();
        for (int index = 0; (index < size); index++)
        {
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
        }
        for (int row = 0; (row < size); row++)
        {
            for (int column = 0; (column < size); column++)
            {
                Add(ref grid, row, column);
                _board[row, column] = blank;
            }
        }
    }

    public void New(ContentPage page, Grid grid)
    {
        _grid = grid;
        _page = page;
        Layout(ref grid);
        _won = false;
        _drawn = false;

        if (single == false)
        {
            if (WhoStarts == 1)
            {
                _piece = new Random().Next(1, 3) % 2 == 0 ? nought : cross;
            }
            else
            {
                _piece = WhoStarts == 0 ? cross : nought;
            }
            page.DisplayAlert(app_title, _piece + " starts", "Ok");
        }
        else
        {
            if (WhoStarts == 1)
            {
                _piece = new Random().Next(1, 3) % 2 == 0 ? nought : cross;
                if(_piece == cross) { ComputerMoves(); }
                page.DisplayAlert(app_title, _piece + " starts", "Ok");

            }
            else if (WhoStarts == 2)
            {
                _piece = nought;
                page.DisplayAlert(app_title, _piece + " starts", "Ok");

            }
            else
            {
                _piece = cross;
                page.DisplayAlert(app_title, _piece + " starts", "Ok");
                ComputerMoves();
            }
        }
    }
    public void ComputerMoves()
    {
        Grid[,] grid = new Grid[3, 3];
        grid[0, 0] = (Grid)_grid.Children[0];
        grid[0, 1] = (Grid)_grid.Children[1];
        grid[0, 2] = (Grid)_grid.Children[2];
        grid[1, 0] = (Grid)_grid.Children[3];
        grid[1, 1] = (Grid)_grid.Children[4];
        grid[1, 2] = (Grid)_grid.Children[5];
        grid[2, 0] = (Grid)_grid.Children[6];
        grid[2, 1] = (Grid)_grid.Children[7];
        grid[2, 2] = (Grid)_grid.Children[8];

        if (Level == 0)
        {
            DoRandom();
        }


        else if (Level == 1)
        {
            if (grid[1, 1].Children.Count < 1)
            {
                grid[1, 1].Children.Add(Piece());
                _board[1, 1] = _piece;
            }
            else if (
                 _board[0, 0] == blank && ((_board[1, 1] == _piece && _board[2, 2] == _piece) ||
                 (_board[1, 0] == _piece && _board[2, 0] == _piece) ||
                 (_board[0, 1] == _piece && _board[0, 2] == _piece)))
            {
                grid[0, 0].Children.Add(Piece());
            }
            else
            {
                DoRandom();
            }
        }


        else
        {
            
                if (grid[2, 2].Children.Count < 1)
                {
                    grid[2, 2].Children.Add(Piece());
                    _board[2, 2] = _piece;
                }
                else if (_board[0, 1] == nought || _board[1, 0] == nought || _board[2, 1] == nought || _board[1, 2] == nought)
                {
                    if (grid[1, 1].Children.Count < 1)
                    {
                        grid[1, 1].Children.Add(Piece());
                        _board[1, 1] = _piece;
                    }
                    else if (_board[1, 1] == cross && grid[0, 0].Children.Count < 1)
                    {
                        grid[0, 0].Children.Add(Piece());
                        _board[0, 0] = _piece;
                    }
                    else if (_board[1, 1] == cross && _board[0, 0] == nought)
                    {
                        if(_board[0, 1] == nought)
                        {
                            if (grid[0,2].Children.Count < 1)
                            {
                                grid[0, 2].Children.Add(Piece());
                                _board[0, 2] = _piece;
                            }
                            else if (grid[2, 1].Children.Count < 1)
                            {
                                grid[2, 1].Children.Add(Piece());
                                _board[2, 1] = _piece;
                            }
                            else if (grid[2, 0].Children.Count < 1)
                            {
                                grid[2, 0].Children.Add(Piece());
                                _board[2, 0] = _piece;
                            }
                        else
                        {
                            DoRandom();

                        }
                    }
                        else if (_board[1, 0] == nought)
                        {
                            if (grid[2, 0].Children.Count < 1)
                            {
                                grid[2, 0].Children.Add(Piece());
                                _board[2, 0] = _piece;
                            }
                            else if (grid[1, 2].Children.Count < 1)
                            {
                                grid[1, 2].Children.Add(Piece());
                                _board[1, 2] = _piece;
                            }
                            else if (grid[0, 2].Children.Count < 1)
                            {
                                grid[0, 2].Children.Add(Piece());
                                _board[0, 2] = _piece;
                            }
                        else
                        {
                            DoRandom();

                        }
                    }
                    else
                    {
                        DoRandom();

                    }
                }
                }


                else if (grid[0, 0].Children.Count < 1)
                {
                    grid[0, 0].Children.Add(Piece());
                    _board[0, 0] = _piece;
                }


                
                else if (_board[2, 0] == nought)
                {
                    if (grid[0, 2].Children.Count < 1)
                    {
                        grid[0, 2].Children.Add(Piece());
                        _board[0, 2] = _piece;
                    }
                    else if (grid[0, 1].Children.Count < 1)
                    {
                        grid[0, 1].Children.Add(Piece());
                        _board[0, 1] = _piece;
                    }
                    else if(grid[1,2].Children.Count<1)
                    {
                        grid[1, 2].Children.Add(Piece());
                        _board[1, 2] = _piece;
                    }
                else
                {
                    DoRandom();

                }
            }
                else if (_board[0, 2] == nought)
                {
                    if (grid[2, 0].Children.Count < 1)
                    {
                        grid[2, 0].Children.Add(Piece());
                        _board[2, 0] = _piece;
                    }
                    else if (grid[1, 0].Children.Count < 1)
                    {
                        grid[1, 0].Children.Add(Piece());
                        _board[1, 0] = _piece;
                    }
                    else if(grid[2,1].Children.Count<1)
                    {
                        grid[2, 1].Children.Add(Piece());
                        _board[2, 1] = _piece;
                    }
                else
                {
                    DoRandom();

                }
            }
                else if (_board[0, 1] == nought)
                {
                    if (grid[2, 1].Children.Count < 1)
                    {
                        grid[2, 1].Children.Add(Piece());
                        _board[2, 1] = _piece;
                    }
                    else if (grid[2, 0].Children.Count < 1)
                    {
                        grid[2, 0].Children.Add(Piece());
                        _board[2, 0] = _piece;
                    }
                    else if (grid[0, 2].Children.Count < 1)
                    {
                        grid[0, 2].Children.Add(Piece());
                        _board[0, 2] = _piece;
                    }
                    else if (grid[1, 2].Children.Count < 1)
                    {
                        grid[1, 2].Children.Add(Piece());
                        _board[1, 2] = _piece;
                    }
                    else if (grid[0, 1].Children.Count < 1)
                    {
                        grid[0, 1].Children.Add(Piece());
                        _board[0, 1] = _piece;
                    }
                else
                {
                    DoRandom();

                }
            }
                else if (grid[1, 1].Children.Count < 1)
            {
                grid[1, 1].Children.Add(Piece());
                _board[1, 1] = _piece;
            }
                else
            {
                DoRandom();
            }
        }




        if (Winner())
        {
            _won = true;
            Show($"{_piece} wins!", app_title);
            if (_piece == nought) { SaveStats(0); }
            else
            {
                SaveStats(2);
            }
        }
        else if (Drawn())
        {
            _drawn = true;
            Show("Droł!", app_title);
            SaveStats(1);
        }
        else
        {
            _piece = (_piece == cross ? nought : cross);
        }
    }
    private void DoRandom()
    {
        int rnd = new Random().Next(0, 9);
        Grid grid = (Grid)_grid.Children[rnd];
        while (grid.Children.Count > 0)
        {
            rnd = new Random().Next(0, 9);
            grid = (Grid)_grid.Children[rnd];
        }
        grid.Children.Add(Piece());
        _board[rnd / 3, rnd % 3] = _piece;
    }
    public async void SaveStats(int isWon)
    {
        if (single == true)
        {
            if (Level == 0)
            {
                int WonNumber = Int32.Parse(StatsL0[isWon]) + 1;
                StatsL0[isWon] = WonNumber.ToString();
            }
            else if (Level == 1)
            {
                int WonNumber = Int32.Parse(StatsL1[isWon]) + 1;
                StatsL1[isWon] = WonNumber.ToString();
            }
            else
            {
                int WonNumber = Int32.Parse(StatsL2[isWon]) + 1;
                StatsL2[isWon] = WonNumber.ToString();
            }
        }
        Application.Current.Properties["StatsL0"] = string.Concat(StatsL0[0]+","+StatsL0[1]+","+StatsL0[2]);
        Application.Current.Properties["StatsL1"] = string.Concat(StatsL1[0]+","+StatsL1[1]+","+StatsL1[2]);
        Application.Current.Properties["StatsL2"] = string.Concat(StatsL2[0]+","+StatsL2[1]+","+StatsL2[2]);
        await Application.Current.SavePropertiesAsync();


    }

}
