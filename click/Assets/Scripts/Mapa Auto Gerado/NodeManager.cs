using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public GameObject objSpriteNode;
    public GameObject objSelectNode;

    private Path path;

    public void SetPath(Path path)
    {

        this.path = path;

        objSpriteNode.transform.localPosition = new Vector3(path.position.x, path.position.y, 0);

        AttNodeSprite();

    }

    public void AttNodeSprite()
    {

        objSpriteNode.GetComponent<SpriteRenderer>().sprite = path.node.sprite;

    }

}
