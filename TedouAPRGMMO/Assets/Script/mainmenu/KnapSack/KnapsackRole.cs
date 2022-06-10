using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnapsackRole : MonoBehaviour
{
    private KnapsackRoleEquip helmEquip;
    private KnapsackRoleEquip clothEquip;
    private KnapsackRoleEquip weaponEquip;
    private KnapsackRoleEquip shoesEquip;
    private KnapsackRoleEquip necklaceEquip;
    private KnapsackRoleEquip braceletEquip;
    private KnapsackRoleEquip ringEquip;
    private KnapsackRoleEquip wingEquip;

    private UILabel hpLabel;
    private UILabel damageLabel;
    private UILabel expLabel;
    private UISlider expSlider;

    private void Awake()
    {
        helmEquip = transform.Find("HelmSprite").GetComponent<KnapsackRoleEquip>();
        clothEquip = transform.Find("ClothSprite").GetComponent<KnapsackRoleEquip>();
        weaponEquip = transform.Find("WeaponSprite").GetComponent<KnapsackRoleEquip>();
        shoesEquip = transform.Find("ShoesSprite").GetComponent<KnapsackRoleEquip>();
        necklaceEquip = transform.Find("NecklaceSprite").GetComponent<KnapsackRoleEquip>();
        braceletEquip = transform.Find("BraceletSprite").GetComponent<KnapsackRoleEquip>();
        ringEquip = transform.Find("RingSprite").GetComponent<KnapsackRoleEquip>();
        wingEquip = transform.Find("WingSprite").GetComponent<KnapsackRoleEquip>();
        
        hpLabel = transform.Find("HpBg/Label").GetComponent<UILabel>();
        damageLabel = transform.Find("DamageBg/Label").GetComponent<UILabel>();
        expLabel = transform.Find("ExpSlider/Label").GetComponent<UILabel>();
        expSlider = transform.Find("ExpSlider").GetComponent<UISlider>();

        PlayerInfo._instance.OnPlayerInfoChanged += this.OnPlayerInfoChanged;
    }

    private void OnDestroy()
    {
        PlayerInfo._instance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
    }

    void OnPlayerInfoChanged(InfoType type)
    {
        if(type == InfoType.All || type == InfoType.HP || type == InfoType.Damage ||
            type == InfoType.Exp || type == InfoType.Equip)
        {
            UpdateShow();
        }
    }

    void UpdateShow()
    {
        PlayerInfo info = PlayerInfo._instance;
        //helmEquip.SetId(info.HelmID);
        //clothEquip.SetId(info.ClothID);
        //weaponEquip.SetId(info.WeaponID);
        //shoesEquip.SetId(info.ShoesID);
        //necklaceEquip.SetId(info.NecklaceID);
        //braceletEquip.SetId(info.BraceletID);
        //ringEquip.SetId(info.RingID);
        //wingEquip.SetId(info.WingID);

        helmEquip.SetInventoryItem(info.helmInventoryItem);
        clothEquip.SetInventoryItem(info.clothInventoryItem);
        weaponEquip.SetInventoryItem(info.weaponInventoryItem);
        shoesEquip.SetInventoryItem(info.shoesInventoryItem);
        necklaceEquip.SetInventoryItem(info.necklaceInventoryItem);
        braceletEquip.SetInventoryItem(info.braceletInventoryItem);
        ringEquip.SetInventoryItem(info.ringInventoryItem);
        wingEquip.SetInventoryItem(info.wingInventoryItem);

        hpLabel.text = info.HP.ToString();
        damageLabel.text = info.Damage.ToString();
        expSlider.value = (float)info.Exp / GameController.GetRequireExpByLevel(info.Level + 1);
        expLabel.text = info.Exp.ToString() + "/" + GameController.GetRequireExpByLevel(info.Level + 1);
    }
}
