using UnityEngine;


	public class Arrow : MonoBehaviour
    {
		public Transform Origin { get { return origin; } set { origin = value; } }

		[SerializeField] private float baseHeight;
		[SerializeField] private RectTransform baseRect;
		[SerializeField] private Transform origin;
		[SerializeField] private bool startsActive;
	[SerializeField] private Camera mainCamera;

		//private Camera mainCamera;
		private RectTransform myRect;
		private Canvas canvas;
		private bool isActive;

		private void Awake ()
		{
			myRect = (RectTransform)transform;
			canvas = GetComponentInParent<Canvas>();
		mainCamera = Camera.main;
			SetActive(startsActive);
		}

		private void Update ()
		{
			if (!isActive)
				return;
			Setup();
		}

		private void Setup ()
		{
			if (origin == null)
				return;
		Debug.Log("origin: " + origin.position);
		Debug.Log("camera: " + mainCamera);
			Vector3 originPosOnScreen = mainCamera.WorldToScreenPoint(origin.position);
		Debug.Log("Test: ");
		Debug.Log(myRect);
			myRect.anchoredPosition = new Vector2(originPosOnScreen.x - Screen.width / 2, originPosOnScreen.y - Screen.height / 2) / canvas.scaleFactor;
			Vector2 differenceToTarget = Input.GetTouch(0).position - (Vector2)originPosOnScreen;
			differenceToTarget.Scale(new Vector2(1f / myRect.localScale.x, 1f / myRect.localScale.y));
			transform.up = differenceToTarget;
			baseRect.anchorMax = new Vector2(baseRect.anchorMax.x, differenceToTarget.magnitude / canvas.scaleFactor / baseHeight);
		}

		private void SetActive (bool b)
		{
			isActive = b;
			if (b)
				Setup();
			baseRect.gameObject.SetActive(b);
		}

		public void Activate () => SetActive(true);
		public void Deactivate () => SetActive(false);
		public void SetupAndActivate (Transform origin)
		{
			Origin = origin;
			Activate();
		}
	}
