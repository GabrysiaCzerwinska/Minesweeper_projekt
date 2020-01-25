# Minesweeper [wersja beta]

## Wstęp
Jest to aplikacja napisana w języku C#, przy użyciu technologi [Windows Presentation Foundation](https://docs.microsoft.com/pl-pl/dotnet/framework/wpf/).
Aplikacja, jest drugą wersją realizującą ten sam temat kierując się innymi kryteriami niż wersja [alpha](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/tree/develop)(duży nacisk na MVVM). Oczywiście logika aplikacji jest przeniesiona z wersji [alpha](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/tree/develop).
## Wygląd
![alt text](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/blob/master/screenshot.png "It looks cool :P")

## Struktura projektu
Projekt jest w formie jednego _solution_, które jest podzielone na poszczególne foldery:
* [`Assets`](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/tree/master/Minesweeper/Assets):Zawiera wszytkie czcionki, zdjęcia użyte w aplikacji
* [`Commands`](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/tree/master/Minesweeper/Commands):  Zawiera klasę `RelayCommand`, która implementuje interfejs `ICommand` i ułatwia jego użycie w aplikacji.
* [`Controls`](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/tree/master/Minesweeper/Controls):  Zawiera kontrolkę o nazwie `FieldControl`, która jest `UserControl` wykorzystującą dostępną w WPF klasę `Button`. Zawiera kilka `DependencyPorperty`, które umożliwiają `Binding` danych z modelu.
* [`Converters`](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/tree/master/Minesweeper/Converters):  Zawiera konwertery, które dbają o dostarczenie poprawnie sformatowanej treści. Konwertery implementują interfejs `IValueConverter`, który udostępnia implementację własnego sposobu na konwersję danych.
* [`Enums`](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/tree/master/Minesweeper/Enums):  Zawiera wyliczeniowy typ danych `GameState`, który ułatwia monitorowanie stanu gry.
* [`Models`](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/tree/master/Minesweeper/Models):  Zawiera klasę `Field`, która reprezentuje nasze "pole" w saperze.
* [`ViewModels`](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/tree/master/Minesweeper/ViewModels):  Zawiera klasę `MainWindowViewModel`, która łączy "View" z naszym modelem. Dodatkowo dostarcza, dla niego, mechanizmy logiki.
* [`Models`](https://github.com/GabrysiaCzerwinska/Minesweeper_projekt/tree/master/Minesweeper/Views): Zawiera kod XAML w którym zapisany jest układ widoku.

## Najważniejsze klasy
### Field
Jest to klasa reprezentująca model "pola" na planszy do gry. Implementuje interfejs `INotifyPropertyChanged`, aby powiadamiać nasz widok o zmianach stanu.
#### Właściwości publiczne
* `Covered` - `bool` daje informację o tym czy pole jest odkryte.
* `DangerLevel` - `int` jest to wartość liczbowa reprezentująca liczbę bomb w pobliżu. Kolor pola w widoku jest uzależniony od tej wartości.
* `IsMine` - `bool` zwraca informację o tym czy pole jest miną. Umożliwa ustawienie pola jako mina poprzez przypisanie wartości `true`
* `IsSuspected` - `bool` zwraca informację o tym czy pole jest podejrzane. Umożliwia ustawienie statusu podejrzanego.
* `FirstClicked` - `bool` informacja o tym czy dane pole zostało naciśnięte jako pierwsze.
#### Metody
* `IncreaseDangerLevel` - `void` zwiększa poziom zagrożenia pola.
* `Reset` - `void` przywraca pole do wartości domyślnej.
### MainWindowViewModel
Klasa jest odpowiedzialna za dostarczenie odpowiednich informacji dla widoku, implementację całej logiki aplikacji a przede wszystkim ma scalać widok z modelem.
#### Właściwości publiczne
* `FieldMargin` - `double` określa wielkość marginesu wokół pola.
* `FieldSize` - `double` określa wielkość pola.
* `FlagCount` - `int` liczba flag do wykorzystania.
* `PlayArea` - `ObservableCollection<Field>` jest to kolekcja pól będących obecnie na planszy. Klasa `ObservableCollection<T>` zawiera implementację intrefejsu `CollectionChanged`, dzięki czemu można zmieniać zawartość kolecji i będzie to miało odzwierciedlenie w widoku.
* `PlayAreaSize` - `int` określa wielkość planszy.
* `DifficultyLevelChange`, `LeftButtonFieldClick`, ` RightButtonFieldClick`, `FaceButtonClick`, `FieldButtonDown` - `ICommand` zapewniają obsługę kliknięć w przyciski.
#### Metody
* `ChangeGameMode(object o)` - umożliwia zmianę poziomu trudności gry na podstawie przekazanego argumentu.
* `ResetGame()` - przywraca grę do stanu początkowego.
* `ResetFields()` - przywraca wartości domyślne wszystkim polom.
* `ResizePlayArea()` - zmienia rozmiar kolekcji w zależności od potrzeby - zmniejsza lub zwiększa.
* `GetModel(FieldControl fieldControl)` - znajduje model, który jest skojarzony z kontrolką przekazaną w argumencie.
* `PlaceMines()` - umieszcza ilość min uzależnioną od poziomu trudności w losowych miejscach i ustawia `DangerLevel` pól sąsiednich.
* `Get1DIndex(int row, int column)` - konwertuje indesky z systemu dwuwymiarowego na system jednowymiarowy. Dzięki temu kod można pisać w systemie dwuwymiarowym(co jest łatwiejsze) i odnieść się do odpowiedniego obiektu w kolekcji `PlayArea`.
* `ShowAllFields()` - pokazuje wszystkie pola.
* `ShowCoveredFields(Field f)` - odkrywa pole, które jest zakryte, lub wszystkie puste pola wokół, jeśli przekazane pole jest puste.
* `LeftButtonField_Click(object sender)` - obsługuje kliknięcie lewego przycisku mszki na polu.
* `CheckVictory()` - sprawdza czy gra została wygrana
* `RightButtonField_Click(object sender)` -  obsługuje kliknięcie prawego przycisku mszki na polu.
* `LeftButtonField_Down(object sender)` - obsługuje wciśnięcie lewego przycisku mszki na polu.
### MainWindow
Klasa reprezentuje widok aplikacji. Wszystko jest tutaj zapisane w XAMLu a odpowiednie dane są zbindowane do ViewModelu.
