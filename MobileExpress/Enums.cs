using System.ComponentModel;

namespace MobileExpress
{
    public enum PaymentMode
    {
        [Description("Non payé")]
        NONE = 0,
        [Description("CB")]
        CB = 1,
        [Description("ESP")]
        ESP = 2,
        [Description("CB/ESP")]
        CBESP = 3,
        [Description("VIR")]
        VIR = 4,
        [Description("CB/VIR")]
        CBVIR = 5,
        [Description("ESP/VIR")]
        ESPVIR = 6,
        [Description("CB/ESP/VIR")]
        CBESPVIR = 7,
    }
    public enum Sexe
    {
        [Description("Femme")]
        Femme = 0,
        [Description("Homme")]
        Homme = 1,
        [Description("Non renseigné")]
        Unknown = 2,
    }
    public enum StockAction
    {
        // 1: Achat, 2: Ajout, 3: Ajout + Achat, 4: Mise à jour, 5: Suppression
        Achat = 1,
        Ajout = 2,
        AchatAjout = 3,
        MiseAJour = 4,
        Suppression = 5,
    }
    public enum TakeOverState
    {
        [Description("En cours")]
        InProgress = 0,
        [Description("Terminé")]
        Done = 1,
        [Description("Récupéré")]
        PickedUp = 2,
        [Description("Annulé")]
        Canceled = 3,
        [Description("Récupéré non réparable")]
        PickedUpIrreparable = 4,
    }
}
