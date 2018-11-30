using UnityEngine;

public class MoveParticlesToTarget : MonoBehaviour {

	public float LifetimeToStartTracking;
	[HideInInspector]public GameObject Target;

	private ParticleSystem system;

	private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

	int count;

	void Start()
	{
		system = GetComponent<ParticleSystem>();

		if (system == null)
		{
			this.enabled = false;
		}
		else
		{
			system.Play();
		}
	}
	void Update()
	{

		count = system.GetParticles(particles);

		for (int i = 0; i < count; i++)
		{
			ParticleSystem.Particle particle = particles[i];
			if (particle.remainingLifetime / particle.startLifetime > LifetimeToStartTracking) continue;
			Vector2 v1 = system.transform.TransformPoint(particle.position);
			Vector2 v2 = Target.transform.position;
			float percent = particle.remainingLifetime / (particle.startLifetime*LifetimeToStartTracking);
			Vector2 tarPosi = Vector2.Lerp(v2, v1,percent);
			particle.position = system.transform.InverseTransformPoint(tarPosi);
			particles[i] = particle;
		}

		system.SetParticles(particles, count);
	}

}
