using System;
using System.Collections.Generic;
//using RosMessageTypes.Map;
using RosMessageTypes.Nav;
using Unity.Robotics.Visualizations;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;
using UnityEngine.UIElements;

public class OccupancyGridCustomDots : BaseVisualFactory<OccupancyGridMsg>
{
    static readonly int k_Color0 = Shader.PropertyToID("_Color0");
    static readonly int k_Color100 = Shader.PropertyToID("_Color100");
    static readonly int k_ColorUnknown = Shader.PropertyToID("_ColorUnknown");
    [SerializeField]
    Vector3 m_Offset = Vector3.zero;
    [SerializeField]
    Material m_Material;
    [SerializeField]
    TFTrackingSettings m_TFTrackingSettings;
    [Header("Dot Colors")]
    [SerializeField]
    Color m_Unoccupied = Color.white;
    [SerializeField]
    Color m_Occupied = Color.black;
    [SerializeField]
    Color m_Unknown = Color.clear;
    [Header("Dot Settings")]
    [SerializeField]
    float m_GridSize = 0.25f;
    [SerializeField]
    float m_DotSize = 0.025f;


    Dictionary<string, OccupancyGridVisual> m_BaseVisuals = new Dictionary<string, OccupancyGridVisual>();

    public override bool CanShowDrawing => true;

    public override IVisual GetOrCreateVisual(string topic)
    {
        OccupancyGridVisual baseVisual;
        if (m_BaseVisuals.TryGetValue(topic, out baseVisual))
            return baseVisual;

        baseVisual = new OccupancyGridVisual(topic, this);
        m_BaseVisuals.Add(topic, baseVisual);
        return baseVisual;
    }

    protected override IVisual CreateVisual(string topic) => throw new NotImplementedException();

    public class OccupancyGridVisual : IVisual
    {
        string m_Topic;
        Mesh m_Mesh;
        Material m_Material;
        Texture2D m_Texture;
        bool m_TextureIsDirty = true;
        bool m_IsDrawingEnabled;
        public bool IsDrawingEnabled => m_IsDrawingEnabled;
        float m_LastDrawingFrameTime = -1;
        float m_DotSize => m_Settings.m_DotSize;

        Drawing3d m_Drawing;
        OccupancyGridCustomDots m_Settings;
        OccupancyGridMsg m_Message;

        public uint Width => m_Message.info.width;
        public uint Height => m_Message.info.height;

        public OccupancyGridVisual(string topic, OccupancyGridCustomDots settings)
        {
            m_Topic = topic;
            m_Settings = settings;

            ROSConnection.GetOrCreateInstance().Subscribe<OccupancyGridMsg>(m_Topic, AddMessage);
        }

        public void AddMessage(Message message)
        {
            if (!VisualizationUtils.AssertMessageType<OccupancyGridMsg>(message, m_Topic))
                return;

            m_Message = (OccupancyGridMsg)message;
            m_TextureIsDirty = true;

            if (m_IsDrawingEnabled && Time.time > m_LastDrawingFrameTime)
                Redraw();

            m_LastDrawingFrameTime = Time.time;
        }

        public void Redraw()
        {
            var origin = m_Message.info.origin.position.From<FLU>();
            var rotation = m_Message.info.origin.orientation.From<FLU>();
            rotation.eulerAngles += new Vector3(0, -90, 0); // TODO: Account for differing texture origin
            var scale = m_Message.info.resolution;

            if (m_Drawing == null)
            {
                m_Drawing = Drawing3dManager.CreateDrawing();
            }
            else
            {
                m_Drawing.Clear();
            }

            m_Drawing.SetTFTrackingSettings(m_Settings.m_TFTrackingSettings, m_Message.header);
            // offset the mesh by half a grid square, because the message's position defines the CENTER of grid square 0,0
            Vector3 drawOrigin = origin - rotation * new Vector3(scale * 0.5f, 0, scale * 0.5f) + m_Settings.m_Offset;

            var step = (m_Settings.m_GridSize / scale);
            int stepInt = (int)step;
            Color32 dotColor;


            for (int i = 0; i < m_Message.info.height; i += stepInt)
            {
                for (int j = 0; j < m_Message.info.width; j += stepInt)
                {
                    if (m_Message.data[i * m_Message.info.width + j] < 0)
                    {
                        dotColor = m_Settings.m_Unknown;
                    }
                    else if (m_Message.data[i * m_Message.info.width + j] < 5)
                    {
                        dotColor = m_Settings.m_Unoccupied;
                    }
                    else 
                    {
                        dotColor = m_Settings.m_Occupied;
                    }
                    Vector3 spherePos = drawOrigin + new Vector3(-i * scale, 0, j * scale);
                    m_Drawing.DrawSphere(spherePos, dotColor, m_DotSize);
                }
            }

            // Color32 originColor = new Color32(byte.MinValue, byte.MinValue, byte.MaxValue, byte.MaxValue);
            // m_Drawing.DrawSphere(drawOrigin, originColor, 0.2f);


        }


        public void DeleteDrawing()
        {
            if (m_Drawing != null)
            {
                m_Drawing.Destroy();
            }

            m_Drawing = null;
        }

        public Texture2D GetTexture()
        {
            return m_Texture;
        }

        public void OnGUI()
        {
            if (m_Message == null)
            {
                GUILayout.Label("Waiting for message...");
                return;
            }

            m_Message.header.GUI();
            m_Message.info.GUI();
        }

        public void SetDrawingEnabled(bool enabled)
        {
            if (m_IsDrawingEnabled == enabled)
                return;

            m_IsDrawingEnabled = enabled;

            if (!enabled && m_Drawing != null)
            {
                m_Drawing.Clear();
            }

            if (enabled && m_Message != null)
            {
                Redraw();
            }
        }
    }
}
