using UnityEngine;

public class Bullet : MonoBehaviour
{
    int bubblePopCount;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bubble"))
        {
            Vector3 hitPoint = collision.contacts[0].point;

            Camera cam = Camera.main;
            Vector3 bubbleScreen = cam.WorldToScreenPoint(collision.collider.bounds.center);
            Vector3 hitScreen = cam.WorldToScreenPoint(hitPoint);

            Vector2 bubbleScreenxy = new Vector2(bubbleScreen.x, bubbleScreen.y);

            float distance = Vector2.Distance(new Vector2(hitScreen.x, hitScreen.y), bubbleScreenxy);

            float radius = (Vector2.Distance(
                new Vector2(cam.WorldToScreenPoint(collision.collider.bounds.min).x,
                    cam.WorldToScreenPoint(collision.collider.bounds.min).y), bubbleScreenxy));

            float normalized = distance / radius;

            int score;
            if (normalized < 0.3f) {score = 50;}
            else if (normalized < 0.7f) {score = 20;}
            else {score = 10;}

            Bubble bubble = collision.collider.GetComponent<Bubble>();
            bubble.Pop(score, bubblePopCount);
            bubblePopCount++;
        }
    }

    
}
