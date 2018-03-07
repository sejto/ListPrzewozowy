# ListPrzewozowy
Program ListPrzewozowy współpracuje z bazą programu PC-Market f-my Insoft.
W bazie Pc-Market utworzono dodatkowe widoki. Program pracuje na własnej bazie tworzonej za pomocą programu KonfiguratorSQL.
Parametry dostępu do bazy zapisane są w rejestrze w formie zaszyfrowanej.
Na chwilę obecna umożliwia utworzenie listy kontrahentów, do których zostanie rozwiezione paliwo oraz przygotowywane sa dokumenty WZ. Dane kontrahenta pobierane z PC-Marketa(w polu uwagi zawarta jest informacja czy kontrahent podpisał oświadczenie oraz upoważnienie do zamykania dokumentu SENT w jego imieniu).
Na dokumencie przewozowym sumowane są ilości zamówionego paliwa z podziałem na ON, ONA, OP. Dodatkowe informacje, które tam sie znajdują to: inny adres dostawy, uwagi, informacja kto zamyka SENT(jeżeli wymagany).
W przygotowaniu jest generowanie dokumentu SENT zastępującego zgłoszenie(tzw procedura awaryjna) oraz generowanie dokumentu xml do zaimportowania w systemie PUESC.

