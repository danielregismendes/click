using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum STATUSPATH
{

    close,
    open,
    visited

}

public class NodeManager : MonoBehaviour
{

    public GameObject nodeArea;
    public GameObject objSpriteNode;
    public GameObject objVisitedNode;
    public SphereCollider sphereCollider;
    public Path path;

    private Animator anim;
    private bool select = false;
    private MapManager mapManager;
    private GameManager gameManager;

    private void Start()
    {
        
        anim = GetComponent<Animator>();
        mapManager = FindFirstObjectByType<MapManager>().GetComponent<MapManager>();
        gameManager = FindFirstObjectByType<GameManager>();

    }

    private void Update()
    {
        
        SetStatusPath();
        InteractNode();

    }

    public void SetPath(Path path)
    {

        this.path = path;

        objSpriteNode.transform.localPosition = new Vector3(path.position.x, path.position.y, 0);

        AttNodeSprite();
        AttColliderPosition();

    }

    public void AttNodeSprite()
    {

        objSpriteNode.GetComponent<SpriteRenderer>().sprite = path.node.sprite;

    }

    public void AttColliderPosition()
    {

        sphereCollider.center = objSpriteNode.transform.localPosition + nodeArea.transform.localPosition;

    }

    public void Select(bool toggle)
    {

        select = toggle;
        anim.SetBool("select", toggle);

    }

    public STATUSPATH GetStatusPath()
    {

        return path.status;

    }

    public void SetStatusPath()
    {

        switch (path.status)
        {
            
            case STATUSPATH.open:

                anim.SetBool("open", true);
                anim.SetBool("close", false);
                anim.SetBool("visited", false);

                break;

            case STATUSPATH.close:

                anim.SetBool("open", false);
                anim.SetBool("close", true);
                anim.SetBool("visited", false);

                break;

            case STATUSPATH.visited:

                anim.SetBool("open", false);
                anim.SetBool("close", false);
                anim.SetBool("visited", true);

                break;

        }

    }

    public void InteractNode()
    {

        switch (path.status)
        {

            case STATUSPATH.open:

                if (select && Input.GetMouseButtonDown(0))
                {

                    mapManager.MapProgress(path);
                    gameManager.LoadFase(path);

                }

                break;

            case STATUSPATH.close:
                break;

            case STATUSPATH.visited:
                break;

        }

    }

}
