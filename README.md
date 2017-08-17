# Pizzeria
Strona WWW pizzerii, na której można zamawiać posiłki. Aplikacja napisana w technologii ASP.NET Core z wykorzystaniem uwierzytelnienia „Individual User Accounts”, w języku C#. Czas implementacji aplikacji: 62 h 32 min. 

Role użytkowników:
•	Zwykły użytkownik („Member”)
•	Pracownik („Employee”)
•	Administrator („Admin”)

Założone konta:
•    Email: member@example.pl
     Hasło: qwerty
     Rola: Member
     
•    Email: employee@example.pl
     Hasło: qwerty
     Rola: Employee
     
•    Email: admin@example.pl
     Hasło: qwerty
     Rola: Admin
    

ZAKŁADKI
Rejestracja:
Podczas rejestracji oprócz adresu email i hasła wymagane jest podanie nr telefonu i adresu dostawy.


Menu:
Menu dzieli się na menu w lokalu oraz menu online.
Produkty podzielone są wg kategorii, a niektóre również wg podkategorii.
Każdy produkt może być dostępny w lokalu i/lub online.
Administrator może dodawać, edytować i usuwać produkty. Ma również dostęp do danych szczegółowych produktu.
Zamawiać można jedynie te produkty, które dostępne są w menu online. 


Wybór dodatkowych składników
Jeśli do produktu, który jest zamawiany można dodać dodatkowe składniki, to lista z takimi składnikami jest wyświetlana i użytkownik może wybrać interesujące go dodatki.


Koszyk
Wybrane przez użytkownika produkty trafiają do koszyka, który tworzy zamówienie. Dane te przechowywane są w sesji. W koszyku można określić ilość każdego produktu. Jeśli do koszyka zostanie dodany produkt, który już się tam znajduje, nie zostanie on dodany jako kolejny produkt, tylko ilość istniejącego zwiększy się o 1. Z koszyka można usuwać produkty.


Formularz dostawy (http Get)
Jest ostatnim krokiem, przy składaniu zamówienia. W nim należy podać adres dostawy (ulica, nr domu, nr mieszkania), imię klienta, telefon oraz email. Jeżeli użytkownik jest zalogowany to email nie jest podawany (został już podany przy rejestracji). Jeżeli użytkownik jest zalogowany jako „Member”, nie podaje również imienia (zostało ono podane przy rejestracji), natomiast adres dostawy oraz telefon są uzupełnione danymi podanymi podczas rejestracji. 

Formularz dostawy (http Post)
Po złożeniu zamówienia przez użytkownika anonimowego lub zalogowanego jako „Member” pojawia się informacja o przyjęciu zamówienia oraz czasie dostawy.


Bieżące zamówienia (dostęp „Employee” i „Admin”)
Po złożeniu zamówienia przez użytkownika zalogowanego jako „Employee” lub „Admin” zostaje on przekierowany na tę stronę. Na niej wyświetlane są wszystkie te zamówienia, które należy obsłużyć (przygotować i dowieźć). Zaznaczenie checkbox’a „zrealizowane” spowoduje zmianę statusu zamówienia, które po chwili zniknie z listy  bieżących zamówień. Ten fragment strony, który wyświetla zamówienia odświeżany jest co 5 sekund w sposób asynchroniczny. Jeśli użytkownik będzie miał otwartą zakładkę „Bieżące zamówienia” to pojawienie się nowego zamówienia sygnalizowane będzie dźwiękiem, a także będzie ono przez chwilę migać. 


Moje zamówienia (dostęp: użytkownik zalogowany)
Historia zamówień danego użytkownika.


Punkty lojalnościowe (dostęp: „Member”)
Wyświetlane są informacje o punktach lojalnościowych oraz o dostępnych środkach pieniężnych otrzymanych za te punkty.
Za każde wydane 20 złotych użytkownik dostaje jeden punkt lojalnościowy. Jeśli użytkownik uzbiera 100 punktów lojalnościowych,  w ramach nagrody otrzymuje 100 zł do wykorzystania przy kolejnych zamówieniach. Natomiast zebrane punkty zmniejszają się o 100.


Zyski (dostęp: „Admin”)
W tej zakładce znajduje się koszt wszystkich zrealizowanych zamówień oraz marże tych zamówień. 


Zarządzanie kontem (dostęp: użytkownik zalogowany)
Możliwość zmiany hasła, numeru telefonu i adresu.




Szczegółowy czas implementacji aplikacji
•	26 min – inicjalizacja projektu oraz dodanie ról i użytkownika – administratora
•	1 h 25 min – Utworzenie tabeli Menu oraz wypełnienie jej danymi
•	10 h 13 min – wyświetlenie menu w lokalu oraz menu online
•	46 min – poprawa błędów związanych ze zmianą nazwy tabeli Menu (na ProductDb)
•	1 h - Dodanie możliwości edycji menu dla Admina (z pomocą automatycznie stworzonego CRUDa przez VS)
•	1 h 36 min - Dodanie możliwości usuwania produktów i sprawdzania szczegółów produktu przez Admina (z pomocą automatycznie stworzonego CRUDa przez VS)
•	7 h 39 min – Utworzenie koszyka
•	2 h 42 min – implementacja formularza dostawy
•	3 h 55 min – implementacja formularza dostawy oraz składanie zamówienia
•	1 h 05 min – implementacja formularza dostawy oraz składanie zamówienia + poprawa błędów
•	7 h 22 min – implementacja zakładki „CurrentOrders”
•	43 min – Wybór drugiego paska menu oraz dodanie go na stronę
•	2 h 54 min - Implementacja zakładki MyOrders
•	37 min - Zmiana struktury tabeli „OrderedProduct” (dzięki temu teraz edycja/usunięcie produktów w menu nie będzie wpływało na zmianę produktów w historii zamówień)
•	1 h 15 min - Refaktoryzacja kodu + niewielkie zmiany treści
•	3 h 55 min - Dodanie punktów lojalnościowych
•	1 h 20 min - Edycja rejestracji (dodanie pól, dodanie roli, dodanie użytkownika do tabeli UsedDb)
•	6 h 20 min - Dodanie możliwości wyboru ilości kupowanych produktów
•	1 h 48 min - Nowe zamówienie pojawiające się w zakładce 'CurrentOrders' sygnalizowane jest za pomocą dźwięku, a także przez chwilę miga
•	22 min - Poniżej kwoty 20 złotych do ceny zamówienia dodawana jest cena za dowóz.
•	2 h 28 min – dodanie zakładki „Profits”
•	44 min - Dodanie w menu nawigacyjnym do odsyłacza do koszyka liczbę produktów znajdujących się w koszyku
•	48 min – Zmiana zakładki „Manage”
•	26 min - Zmiana wyświetlanej treści (dodanie tłumaczeń na polski, dodanie DataAnnotations, zmiany w menu dla zalogowanych użytkowników)
•	35 min - Automatyczne utworzenie trzech użytkowników o różnych rolach
