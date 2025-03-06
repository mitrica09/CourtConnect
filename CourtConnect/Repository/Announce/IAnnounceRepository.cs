using CourtConnect.ViewModel.Announce;

namespace CourtConnect.Repository.Announce
{
    public interface IAnnounceRepository
    {
        Task<bool> Create(AnnounceFormViewModel announceForm);

        Task<List<AnnounceForDisplayViewModel>> GetAllAnnounces();

        Task<AnnounceDetailsViewModel> GetAnnounceDetails(int announceId,string userId);
    }
}

/* 
 Sa iti arunce o exceptie ca nu esti logat daca vrei sa creezi un anunt fara a fi logat, sa te redirectioneze catre login
 De facut:
- Creare anunt(deja facut), dar trebuie modificat in felul urmator:
1)la end date la anunt sa nu te lase sa pui o data mai mare decat inceperea jocului, ba chiar sa fie cu o zi mai devreme decat inceperea meciului. Iar daca
se ajunge la acea data anuntul sa se stearga.
Daca anuntul depaseste timpul de end date sa nu mai poata fi accesat si statusul sa se schimbe in "expirat"

- sa poti sa pui doar un singur anunt per utilizator

- sa poti sa nu mai scoti anuntul daca este deja acceptat de cineva, decat daca te intelegi cu jucatorul respectiv

-Sa zicem ca eu postez un anunt, un jucator vine si imi accepta anuntul, eu ar trebui sa primesc notificare si sa confirm si eu la randul meu ca vreau sa ma joc
cu jucatorul care mi-a acceptat anuntul (adica sa ai posibilitatea de a refuza poate un player care nu ti convine ca e mai bun sau prea slab)

-In momentul in care anuntul este acceptat de ambii playeri statusul anuntului sa se schimbe in "Meci acceptat"
-Sa am un tab cu meciurile mele in care sa afisez data, ora, locatia si adversarul
-In momentul in care se ajunge la data si la ora stabilite in anunt, statusul anuntului sa se schimbe "in desfasurare".
-Dupa se joaca meciul si ramane in desfasurare pana cei 2 playeri trec scorurile in pagina de match. Daca rezultatele sunt diferite, nu se vor valida si meciul nu se va inchide.

Cum sa fac aici sa intervina aplicatia in caz ca cei 2 playeri nu se inteleg si trec scoruri diferite?
1)Dupa 12 de ore playerul care nu trece scorul va primi un avertisment ca notificare, iar dupa 24 de ore va primi o penalitate majora de puncte.

-Dupa ce se trec rezultatele corect si totul este validat, statusul se schimba in "meci terminat si eventual sa scrie si castigatorul meciului".
-Daca se poate in pagina in care este matchul sa adaug si o sectiunde de chat pentru ca jucatorii sa poata comunica mai bine (exemplu cel.ro)
 */