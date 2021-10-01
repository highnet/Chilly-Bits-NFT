using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Linq;
using System;

public class AttributeRandomizer : MonoBehaviour
{
    public int rngDiceSize;
    public InputField batchAmountInput;
    public InputField batchStartRenderInput;
    public int batchJobAmount;
    int commonThreshold = 40;
    int uncommon_Threshold = 60;
    int rare_Threshold = 70;
    int epic_Threshold = 95;
    int legendaryThreshold = 100;
    public int cardSetsAmount = 1;
    int cardSetCounter = 0;
    public List<GameObject> hats_uncommon;
    public List<GameObject> hats_common;
    public List<GameObject> hats_rare;
    public List<GameObject> hats_epic;
    public List<GameObject> hats_legendary;
    public List<GameObject> weapons_uncommon;
    public List<GameObject> weapons_common;
    public List<GameObject> weapons_rare;
    public List<GameObject> weapons_epic;
    public List<GameObject> weapons_legendary;
    public List<GameObject> eyeballs_uncommon;
    public List<GameObject> eyeballs_common;
    public List<GameObject> eyeballs_rare;
    public List<GameObject> eyeballs_epic;
    public List<GameObject> eyeballs_legendary;
    public List<GameObject> eyebrows_uncommon;
    public List<GameObject> eyebrows_common;
    public List<GameObject> eyebrows_rare;
    public List<GameObject> eyebrows_epic;
    public List<GameObject> eyebrows_legendary;
    public List<GameObject> beaks_uncommon;
    public List<GameObject> beaks_common;
    public List<GameObject> beaks_rare;
    public List<GameObject> beaks_epic;
    public List<GameObject> beaks_legendary;
    public List<GameObject> accesories_uncommon;
    public List<GameObject> accesories_common;
    public List<GameObject> accesories_rare;
    public List<GameObject> accesories_epic;
    public List<GameObject> accesories_legendary;
    public List<GameObject> playingCards;
    public Queue<GameObject> cardStack;
    public List<Material> colors_uncommon;
    public List<Material> colors_common;
    public List<Material> colors_rare;
    public List<Material> colors_epic;
    public List<Material> colors_legendary;
    public List<Material> fullScreenShaders_common;
    public List<Material> fullScreenShaders_uncommon;
    public List<Material> fullScreenShaders_rare;
    public List<Material> fullScreenShaders_epic;
    public List<Material> fullScreenShaders_legendary;
    public Character mainCharacter;
    public GameObject camRenderTex;
    public GameObject fullScreenShaderTex;
    public GameObject wall;
    public GameObject floor;
    public Camera renderCam;
    public int renderNumber;
    public int startingRenderNumber;
    public GameObject body;
    public GameObject belly;
    public GameObject visor;
    public Material bypassFilter;
    Hashtable renderedPingu;
    public GameObject uiGameobject;
    StringBuilder csvStringBuilder;

    private void Start()
    {
        csvStringBuilder = new StringBuilder();
        batchJobAmount = 0;
        renderNumber = 1;
        startingRenderNumber = 1;
        cardSetCounter = 0;
        cardStack = new Queue<GameObject>();
        startingRenderNumber = 1;
        RebuildCardStack();
        Randomize();
        renderedPingu = new Hashtable();
        uiGameobject.SetActive(true);
    }

    public void ExportPinguToCsvBuffer()
    {
        csvStringBuilder.AppendLine();
        csvStringBuilder.Append(renderNumber + ", " + mainCharacter.hat + ", " + mainCharacter.weapon + ", " + mainCharacter.wallColor + ", " + mainCharacter.floorColor + ", " + mainCharacter.eyeball + ", " + mainCharacter.beak + ", " + mainCharacter.eyebrow + ", " + mainCharacter.accesory + ", " + mainCharacter.bodyColor + ", " + mainCharacter.visorColor + ", " + mainCharacter.appliedFullScreenShader + ", " + mainCharacter.bellyColor);
    }

    public void SaveCsvBufferToFile()
    {
        string csvBufferString = csvStringBuilder.ToString();
        TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
        TimeSpan unixTicks = new TimeSpan(DateTime.Now.Ticks) - epochTicks;
        Int32 unixTimestamp = (Int32)unixTicks.TotalSeconds;

        StreamWriter writer;

        if (Application.isEditor)
        {
            writer = new StreamWriter(Application.dataPath + "/renders/log" + unixTimestamp + ".txt", false);
        } else
        {
            writer = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + "/renders/log" + unixTimestamp + ".txt", false);
        }

        using (writer)
        {
            writer.Write(csvBufferString);
        }

    }

    public void BatchRender()
    {
        renderNumber = startingRenderNumber;
        cardSetCounter = 0;
        csvStringBuilder = new StringBuilder();
        renderedPingu = new Hashtable();
        StartCoroutine("BatchRenderAction");
    }

    public void RenderCamera()
    {
        string keyString = mainCharacter.hat + mainCharacter.weapon + mainCharacter.wallColor + mainCharacter.floorColor + mainCharacter.eyeball + mainCharacter.beak + mainCharacter.eyebrow + mainCharacter.accesory + mainCharacter.bodyColor + mainCharacter.visorColor + mainCharacter.bellyColor + mainCharacter.appliedFullScreenShader;
        if (!renderedPingu.ContainsKey(keyString))
        {
            renderedPingu.Add(renderNumber, keyString);
            RenderTexture outputMapTexture = new RenderTexture(800, 800, 32);
            outputMapTexture.name = "RT" + renderNumber;
            renderCam.targetTexture = outputMapTexture;
            RenderTexture currentCameraViewTexture = RenderTexture.active;
            RenderTexture.active = renderCam.targetTexture;
            renderCam.Render();
            Texture2D cameraImage = new Texture2D(renderCam.targetTexture.width, renderCam.targetTexture.height, TextureFormat.RGB24, false, true);
            cameraImage.ReadPixels(new Rect(0, 0, renderCam.targetTexture.width, renderCam.targetTexture.height), 0, 0);
            cameraImage.Apply();
            RenderTexture.active = currentCameraViewTexture;
            var imageBytes = cameraImage.EncodeToPNG();
            Destroy(cameraImage);


            if (Application.isEditor)
            {
                File.WriteAllBytes(Application.dataPath + "/renders/" + renderNumber + "_" + keyString + ".png", imageBytes);
            } else
            {
                File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + "/renders/" + renderNumber + "_" + keyString + ".png", imageBytes);
            }
                 
            renderCam.targetTexture = null;
            Destroy(outputMapTexture);
        }
        renderNumber++;
    }

    public void Randomize()
    {
        Debug.Log("=Randomizing a Pingu=");
        ClearAttributes();
        RandomizeAttribute("beak", beaks_common, beaks_uncommon, beaks_rare, beaks_epic, beaks_legendary);
        RandomizeAttribute("eyeball", eyeballs_common, eyeballs_uncommon, eyeballs_rare, eyeballs_epic, eyeballs_legendary);
        RandomizeAttribute("eyebrow", eyebrows_common, eyebrows_uncommon, eyebrows_rare, eyebrows_epic, eyebrows_legendary);
        RandomizeMaterial("wallcolor", wall, colors_common, colors_uncommon, colors_rare, colors_epic, colors_legendary);
        RandomizeMaterial("floorcolor", floor, colors_common, colors_uncommon, colors_rare, colors_epic, colors_legendary);
        RandomizeMaterial("bodycolor", body, colors_common, colors_uncommon, colors_rare, colors_epic, colors_legendary);
        RandomizeMaterial("visorcolor", visor, colors_common, colors_uncommon, colors_rare, colors_epic, colors_legendary);
        RandomizeMaterial("bellycolor", belly, colors_common, colors_uncommon, colors_rare, colors_epic, colors_legendary);


        int randomNumberOfAttributesRoll = UnityEngine.Random.Range(0, rngDiceSize);
        int randomNumberOfAttributes;


        if (randomNumberOfAttributesRoll < commonThreshold)
        {
            randomNumberOfAttributes = 0;
        }
        else if (randomNumberOfAttributesRoll < uncommon_Threshold)
        {
            randomNumberOfAttributes = 1;
        }
        else if (randomNumberOfAttributesRoll < rare_Threshold)
        {
            randomNumberOfAttributes = 3;
        }
        else if (randomNumberOfAttributesRoll < epic_Threshold)
        {
            randomNumberOfAttributes = 2;
        }
        else { randomNumberOfAttributes = 4; }

        Debug.Log(randomNumberOfAttributesRoll);
        Debug.Log(randomNumberOfAttributes);

        List<String> possibleAttributes = new List<string>
        {
            UnityEngine.Random.Range(0, 100) + "->hat",
            UnityEngine.Random.Range(0, 90) + "->weapon",
            UnityEngine.Random.Range(0, 80) + "->accessory",
            UnityEngine.Random.Range(0, 60) + "->fullScreenShader"
        };
        possibleAttributes.Sort();
        List<String> attributesToApply = new List<string>();
        for (int i = 0; i < randomNumberOfAttributes; i++)
        {
            string s = possibleAttributes[possibleAttributes.Count - 1];
            attributesToApply.Add(s);
            possibleAttributes.RemoveAt(possibleAttributes.Count - 1);
        }
        if (attributesToApply.Any(e => e.Contains("hat")))
        {
            RandomizeAttribute("hat", hats_common, hats_uncommon, hats_rare, hats_epic, hats_legendary);
        }
        if (attributesToApply.Any(e => e.Contains("weapon")))
        {
            RandomizeAttribute("weapon", weapons_common, weapons_uncommon, weapons_rare, weapons_epic, weapons_legendary);
        }
        if (attributesToApply.Any(e => e.Contains("accessory")))
        {
            RandomizeAttribute("accessory", accesories_common, accesories_uncommon, accesories_rare, accesories_epic, accesories_legendary);
        }
        if (attributesToApply.Any(e => e.Contains("fullScreenShader")))
        {
            RandomizeMaterial("appliedFullScreenShader", fullScreenShaderTex, fullScreenShaders_common, fullScreenShaders_uncommon, fullScreenShaders_rare, fullScreenShaders_epic, fullScreenShaders_legendary);
        }
    }
    private void ClearAttributes()
    {
        foreach (GameObject go in hats_uncommon)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in hats_common)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in hats_rare)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in hats_epic)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in hats_legendary)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in weapons_common)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in weapons_uncommon)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in weapons_rare)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in weapons_epic)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in weapons_legendary)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in eyeballs_uncommon)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in eyeballs_common)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in eyeballs_rare)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in eyeballs_epic)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in eyeballs_legendary)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in eyebrows_uncommon)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in eyebrows_common)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in eyebrows_rare)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in eyebrows_epic)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in eyebrows_legendary)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in beaks_uncommon)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in beaks_common)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in beaks_rare)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in beaks_epic)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in beaks_legendary)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in accesories_uncommon)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in accesories_common)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in accesories_rare)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in accesories_epic)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in accesories_legendary)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in playingCards)
        {
            go.SetActive(false);
        }
        mainCharacter.hat = "";
        mainCharacter.weapon = "";
        mainCharacter.wallColor = "";
        mainCharacter.floorColor = "";
        mainCharacter.eyeball = "";
        mainCharacter.beak = "";
        mainCharacter.eyebrow = "";
        mainCharacter.accesory = "";
        mainCharacter.bodyColor = "";
        mainCharacter.visorColor = "";
        mainCharacter.bellyColor = "";
        fullScreenShaderTex.GetComponent<Renderer>().material = bypassFilter;
        mainCharacter.appliedFullScreenShader = "";
    }


    private void RandomizeAttribute(string attributeType, List<GameObject> commons, List<GameObject> uncommons, List<GameObject> rares, List<GameObject> epics, List<GameObject> legendaries)
    {
        int roll = UnityEngine.Random.Range(0, rngDiceSize);
        int seed;
        if (roll < commonThreshold)
        {
            seed = UnityEngine.Random.Range(0, commons.Count);
            commons[seed].SetActive(true);
            mainCharacter.setAttributeReference(attributeType, commons[seed].name);
        }
        else if (roll < uncommon_Threshold)
        {
            seed = UnityEngine.Random.Range(0, uncommons.Count);
            uncommons[seed].SetActive(true);
            mainCharacter.setAttributeReference(attributeType, uncommons[seed].name);
        }
        else if (roll < rare_Threshold)
        {
            seed = UnityEngine.Random.Range(0, rares.Count);
            rares[seed].SetActive(true);
            mainCharacter.setAttributeReference(attributeType, rares[seed].name);
        }
        else if (roll < epic_Threshold)
        {
            seed = UnityEngine.Random.Range(0, epics.Count);
            epics[seed].SetActive(true);
            mainCharacter.setAttributeReference(attributeType, epics[seed].name);
        }
        else if (roll < legendaryThreshold)
        {
            seed = UnityEngine.Random.Range(0, legendaries.Count);
            legendaries[seed].SetActive(true);
            mainCharacter.setAttributeReference(attributeType, legendaries[seed].name);
        }

    }

    private void RandomizeMaterial(string attributeType, GameObject go, List<Material> commons, List<Material> uncommons, List<Material> rares, List<Material> epics, List<Material> legendaries)
    {
        int roll = UnityEngine.Random.Range(0, rngDiceSize);
        int seed;
        if (roll < commonThreshold)
        {
            seed = UnityEngine.Random.Range(0, commons.Count);
            go.GetComponent<Renderer>().material = commons[seed];
            mainCharacter.setAttributeReference(attributeType, commons[seed].name);
        }
        else if (roll < uncommon_Threshold)
        {
            seed = UnityEngine.Random.Range(0, uncommons.Count);
            go.GetComponent<Renderer>().material = uncommons[seed];

            mainCharacter.setAttributeReference(attributeType, uncommons[seed].name);
        }
        else if (roll < rare_Threshold)
        {
            seed = UnityEngine.Random.Range(0, rares.Count);
            go.GetComponent<Renderer>().material = rares[seed];
            mainCharacter.setAttributeReference(attributeType, rares[seed].name);
        }
        else if (roll < epic_Threshold)
        {
            seed = UnityEngine.Random.Range(0, epics.Count);
            go.GetComponent<Renderer>().material = epics[seed];
            mainCharacter.setAttributeReference(attributeType, epics[seed].name);
        }
        else if (roll < legendaryThreshold)
        {
            seed = UnityEngine.Random.Range(0, legendaries.Count);
            go.GetComponent<Renderer>().material = legendaries[seed];
            mainCharacter.setAttributeReference(attributeType, legendaries[seed].name);
        }
    }

    public void ReplaceWeaponWithCard()
    {
        foreach (GameObject go in weapons_common)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in weapons_uncommon)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in weapons_rare)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in weapons_epic)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in weapons_legendary)
        {
            go.SetActive(false);
        }

        GameObject drawnCard = cardStack.Dequeue();
        drawnCard.SetActive(true);
        mainCharacter.setAttributeReference("weapon", drawnCard.name);

        if (cardStack.Count == 0)
        {
            RebuildCardStack();
            cardSetCounter++;
        }
    }

    public void RebuildCardStack()
    {
        foreach (GameObject go in playingCards)
        {
            cardStack.Enqueue(go);
        }
    }

    public void UpdateBatchJobAmountWithUI()
    {
        string inputString = batchAmountInput.text;
        int.TryParse(inputString, out batchJobAmount);
        if (batchJobAmount < 0)
        {
            Debug.LogWarning("Series run # should be 0 or higher !! Setting to 0");
        }
        else if (batchJobAmount > 1000)
        {
  
            Debug.Log("SAFETY MODE ON !!!");
            Debug.LogWarning("to render more than 1000 objects turn safety mode off in code !! Setting to 1000");
            batchJobAmount = 1000;
        }
    }


    public void UpdateBatchStartRenderNumberWithUI()
    {
        string inputString = batchStartRenderInput.text;
        int.TryParse(inputString, out startingRenderNumber);
        if (startingRenderNumber < 1)
        {
            Debug.LogWarning(" Series start # should be 1 or higher !! Setting to 1 ");
            startingRenderNumber = 1;
        }
    }

    IEnumerator BatchRenderAction()
    {
        uiGameobject.SetActive(false);
        csvStringBuilder.Clear();
        csvStringBuilder = new StringBuilder("render, hat, weapon, wallcolor, floorcolor, eyeball, beak, eyebrow, accessory, bodycolor, visorcolor, fullscreenshader, bellycolor");
        for (int i = 0; i < batchJobAmount; i++)
        {
            Randomize();

            if (cardSetCounter < cardSetsAmount)
            {
                ReplaceWeaponWithCard();
            }

            yield return new WaitForSeconds(0.2f);
            ExportPinguToCsvBuffer();
            RenderCamera();
        }
        uiGameobject.SetActive(true);
        SaveCsvBufferToFile();
    }
}
