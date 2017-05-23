using System;
using System.Collections.Generic;

using UnityEngine;

using ksubaka.core.logging;
using ksubaka.core.services;
using ksubaka.core.utils;
using ksubaka.core.unity;
using ksubaka.core.unity.io;
using ksubaka.propertystore;
using ksubaka.tools;

namespace betomorrow.tmn.shootinggallery {

public class TextureInjector : MonoBehaviour {

	static readonly Logger _logger = LoggerManager.GetLogger<TextureInjector>();

	[Serializable]
	class TextureDefinition {
		[SerializeField] [Type(typeof(AssetUri))] Property _url;

		[SerializeField] Texture _fallback;

		public void Load(TextureLoader loader, Action<Texture> callback) {
			var textureUri = _url.GetValue<AssetUri>();
			loader.LoadTexture(textureUri,
				texture => NotifyLoaded(texture, callback, "Loaded texture from '{0}'", textureUri),
				error => NotifyLoaded(_fallback, callback, "Falled back to default texture because of texture load error from '{0}' : {1}", textureUri, error)
			);
		}

		void NotifyLoaded(Texture texture, Action<Texture> callback, string format, params object[] args) {
			_logger.Info(format, args);
			callback(texture);
		}
	}

	[SerializeField] Renderer[] _targetRenderers;

	[SerializeField] TextureDefinition[] _textures;

	IList<Texture> _loadedTextures = new List<Texture>();

	void Awake() {
		var coroutineRunner = ServiceRepository.GetService<CoroutineRunner>();
		var loader = new TextureLoader(coroutineRunner);
		_textures.ForEach(t => t.Load(loader, _loadedTextures.Add));
		coroutineRunner.InvokeAfter(ApplyTextures, () => _loadedTextures.Count < _textures.Length);
	}

	void ApplyTextures() {
		foreach (var renderer in _targetRenderers) {
			renderer.material.mainTexture = _loadedTextures.GetRandom();
		}
	}
}
}
