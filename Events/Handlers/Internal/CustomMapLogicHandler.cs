using ProjectMER.Events.Handlers;
using ProjectMER.Events.Arguments;
using LabApi.Features.Wrappers;
using System.Linq;

namespace ProjectMER.Events.Handlers.Internal;

public class CustomMapLogicHandler
{
    public void Enable()
    {
        // Buton etkileşim olayına abone ol
        Schematic.ButtonInteracted += OnButtonInteracted;
    }

    public void Disable()
    {
        // Olay aboneliğini temizle
        Schematic.ButtonInteracted -= OnButtonInteracted;
    }

    private void OnButtonInteracted(ButtonInteractedEventArgs ev)
    {
        // 1. Şematik adını kontrol et (Unity'de şematik kök objesine verdiğin isim, örn: "CustomDoor")
        if (ev.Schematic.Name == "CustomDoor")
        {
            // 2. Buton adını kontrol et (Unity'deki Pickup objesinin adı, örn: "AccessButton")
            if (ev.Button.GameObject.name == "AccessButton")
            {
                // 3. Kart gereksinimini kontrol et (Envanterde Scientist veya O5 kartı olmalı)
                bool holdsCard = ev.Player.CurrentItem?.Type == ItemType.KeycardScientist || 
                                 ev.Player.CurrentItem?.Type == ItemType.KeycardO5;

                bool hasCardInInventory = ev.Player.Inventory.Any(item => 
                    item.ItemType == ItemType.KeycardScientist || item.ItemType == ItemType.KeycardO5);

                if (holdsCard || hasCardInInventory)
                {
                    ev.Player.ShowHint("<color=green>Erişim Onaylandı!</color>", 2f);
                    
                    // 4. "OpenDoor" animasyonunu ilk Animator üzerinde oynat
                    ev.Schematic.AnimationController.Play("OpenDoor", 0);
                }
                else
                {
                    ev.Player.ShowHint("<color=red>Erişim Engellendi! Kart gerekli.</color>", 3f);
                }
            }
        }
    }
}
