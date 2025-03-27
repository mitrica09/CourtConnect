using CourtConnect.ViewModel.Announce;

namespace CourtConnect.Repository.Announce
{
    public interface IAnnounceRepository
    {
        Task<bool> Create(AnnounceFormViewModel announceForm);

        Task<List<AnnounceForDisplayViewModel>> GetAllAnnounces();

        Task<AnnounceDetailsViewModel> GetAnnounceDetails(int announceId,string userId);
        Task<bool> ConfirmGuest(int announceId, string userId);
        Task<bool> ConfirmHost(int announceId, string userId);
        Task<List<AnnounceDetailsViewModel>> GetMyAnnounces();
        Task<bool> RejectGuest(int announceId, string userId);
        Task<int?> CreateMatch(int announceId);

    }
}

/* 
 De facut:
- Creare anunt(deja facut), dar trebuie modificat in felul urmator:
1)la end date la anunt sa nu te lase sa pui o data mai mare decat inceperea jocului, ba chiar sa fie cu o zi mai devreme decat inceperea meciului. Iar daca
se ajunge la acea data anuntul sa se stearga.
Daca anuntul depaseste timpul de end date sa nu mai poata fi accesat si statusul sa se schimbe in "expirat"

- sa poti sa pui doar un singur anunt per utilizator

-In momentul in care se ajunge la data si la ora stabilite in anunt, statusul anuntului sa se schimbe "in desfasurare".

-Dupa se joaca meciul si ramane in desfasurare pana cei 2 playeri trec scorurile in pagina de match. Daca rezultatele sunt diferite, nu se vor valida si meciul nu se va inchide.

Cum sa fac aici sa intervina aplicatia in caz ca cei 2 playeri nu se inteleg si trec scoruri diferite?
1)Dupa 12 de ore playerul care nu trece scorul va primi un avertisment ca notificare, iar dupa 24 de ore va primi o penalitate majora de puncte.

-Dupa ce se trec rezultatele corect si totul este validat, statusul se schimba in "meci terminat si eventual sa scrie si castigatorul meciului".

-Daca se poate in pagina in care este matchul sa adaug si o sectiunde de chat pentru ca jucatorii sa poata comunica mai bine (exemplu cel.ro)
 */