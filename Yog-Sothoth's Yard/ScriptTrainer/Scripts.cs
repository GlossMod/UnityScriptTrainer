using System;
using Example;
using HotelModule;
using UnityEngine;
using UnityGameUI;

namespace ScriptTrainer;
public class Scripts
{
    // 添加现金
    public static void AddMoney()
    {
        UIWindows.SpawnInputDialog("你想增加多少现金", "添加", "10000", (string num) =>
        {
            HotelAttributes.Instance.ModifyAttriBute(AttributesECurrencyEnum.E_Money, HotelBuffEBuffEffect.E_ExtraIncrease, num.ConvertToLongDef(100));
        });
    }

    // 添加San
    public static void AddSan()
    {
        UIWindows.SpawnInputDialog("你想增加多少San值", "添加", "100", (string num) =>
        {
            HotelAttributes.Instance.ModifyAttriBute(AttributesECurrencyEnum.E_San, HotelBuffEBuffEffect.E_ExtraIncrease, num.ConvertToLongDef(100));
        });
    }

    // 添加灵魂
    public static void AddSouls()
    {
        UIWindows.SpawnInputDialog("你想增加多少灵魂", "添加", "500", (string num) =>
        {
            HotelAttributes.Instance.ModifyAttriBute(AttributesECurrencyEnum.E_Souls, HotelBuffEBuffEffect.E_ExtraIncrease, num.ConvertToLongDef(500));
        });
    }

    // 添加 清洁度
    public static void AddCleanliness()
    {
        UIWindows.SpawnInputDialog("你想增加多少清洁度", "添加", "500", (string num) =>
        {
            HotelAttributes.Instance.ModifyAttriBute(AttributesECurrencyEnum.E_Cleanliness, HotelBuffEBuffEffect.E_ExtraIncrease, num.ConvertToLongDef(500));
        });
    }

    // 降低恶值
    public static void ReduceEvil()
    {
        UIWindows.SpawnInputDialog("你想增加多少清洁度", "添加", "100", (string num) =>
        {
            HotelAttributes.Instance.ModifyAttriBute(AttributesECurrencyEnum.E_Evil, HotelBuffEBuffEffect.E_Reduce, num.ConvertToLongDef(100));
        });
    }

    // 添加行动力
    public static void AddActionPiont()
    {
        UIWindows.SpawnInputDialog("你想增加多少行动力", "添加", "1", (string num) =>
        {
            HotelAttributes.Instance.ModifyAttriBute(AttributesECurrencyEnum.E_ActionPiont, HotelBuffEBuffEffect.E_ExtraIncrease, num.ConvertToLongDef(1));
        });
    }

    // 耶芙娜好感度
    public static void AddDragonFavor()
    {
        UIWindows.SpawnInputDialog("你想增加多少耶芙娜好感度", "添加", "100", (string num) =>
        {
            HotelAttributes.Instance.ModifyAttriBute(AttributesECurrencyEnum.E_DragonFavor, HotelBuffEBuffEffect.E_ExtraIncrease, num.ConvertToLongDef(100));
        });
    }

    // 小叶子好感度
    public static void AddMaidFavor()
    {
        UIWindows.SpawnInputDialog("你想增加多少小叶子好感度", "添加", "100", (string num) =>
        {
            HotelAttributes.Instance.ModifyAttriBute(AttributesECurrencyEnum.E_MaidFavor, HotelBuffEBuffEffect.E_ExtraIncrease, num.ConvertToLongDef(100));
        });
    }

    // 特莉波卡好感度
    public static void AddDeathFavor()
    {
        UIWindows.SpawnInputDialog("你想增加多少特莉波好感度", "添加", "100", (string num) =>
        {
            HotelAttributes.Instance.ModifyAttriBute(AttributesECurrencyEnum.E_DeathFavor, HotelBuffEBuffEffect.E_ExtraIncrease, num.ConvertToLongDef(100));
        });
    }

    // 霞露零好感度
    public static void AddElfFavor()
    {
        UIWindows.SpawnInputDialog("你想增加多少霞露零好感度", "添加", "100", (string num) =>
        {
            HotelAttributes.Instance.ModifyAttriBute(AttributesECurrencyEnum.E_ElfFavor, HotelBuffEBuffEffect.E_ExtraIncrease, num.ConvertToLongDef(100));
        });
    }
}