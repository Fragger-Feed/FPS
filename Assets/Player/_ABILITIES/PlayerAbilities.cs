using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    // == Data ==
    [SerializeField] public Abilities_Data AbilitiesData;

    // == Additional ==
    public GameObject AbilitySample;
    public GameObject AbilityLists;


    private void Start() {
        ReloadAbilityRender();
    }


    public void AddAbility(string Ability) {
        if (AbilitiesData.OccupiedMemory + AbilitiesData.AllAbilities[Ability].Size <= AbilitiesData.AvailableMemory && !AbilitiesData.CurrentAbilities.Contains(Ability)) {
            AbilitiesData.OccupiedMemory += AbilitiesData.AllAbilities[Ability].Size;
            AbilitiesData.CurrentAbilities.Add(Ability);
        }
    }
    public void DeleteAbility(string Ability) {
        if (AbilitiesData.CurrentAbilities.Contains(Ability)) {
            AbilitiesData.OccupiedMemory -= AbilitiesData.AllAbilities[Ability].Size;
            AbilitiesData.CurrentAbilities.Remove(Ability);
        }
    }

    public void ReloadAbilityRender() {
        for (int i = 0; i < AbilityLists.transform.childCount; i++) {
            Destroy(AbilityLists.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < AbilitiesData.CurrentAbilities.Count; i++) {
            GameObject AbilityObj = Instantiate(AbilitySample, AbilityLists.transform);
            AbilityObj.transform.localPosition = new Vector3(i * -55, 0, 0);
            Texture2D tex = AbilitiesData.AllAbilities[AbilitiesData.CurrentAbilities[i]].SmallTexture;
            if (tex != null) {
                AbilityObj.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

            }
        }
    }
}
