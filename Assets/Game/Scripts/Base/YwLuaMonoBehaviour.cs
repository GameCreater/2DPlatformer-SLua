﻿/**
 * The lua mono behavior class.
 *
 * @filename  YwLuaMonoBehaviour.cs
 * @copyright Copyright (c) 2015 Yaukey/yaukeywang/WangYaoqi (yaukeywang@gmail.com) all rights reserved.
 * @license   The MIT License (MIT)
 * @author    Yaukey
 * @date      2015-11-27
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLua;

// The lua mono behaviour class.
public class YwLuaMonoBehaviour : YwLuaMonoBehaviourBase
{
    // The mono method types.
    public enum EMonoMethod
    {
        Start,
        Update,
        LateUpdate,
        FixedUpdate,
        LiteUpdate,
        OnEnable,
        OnDisable,
        OnDestroy,
        OnTriggerEnter,
        OnTriggerEnter2D,
        OnTriggerExit,
        OnTriggerExit2D,
        // Add more if your need.
    }

    // The lua class name.
    public string m_className = string.Empty;

    // All mono method we have.
    public List<EMonoMethod> m_monoMethods = new List<EMonoMethod>(); // System.Enum.GetValues(typeof(EFunction)).Length

    // The parameters used to pass to lua.
    public GameObject[] m_parameters = null;

    // Name of enable base lua function.
    private static readonly string LUA_FUNC_NAME_ON_ENABLE_BASE = "OnEnableBase";

    // Name of disable base lua function.
    private static readonly string LUA_FUNC_NAME_ON_DISENABLE_BASE = "OnDisableBase";

    // The On enable base function.
    private LuaFunction m_cOnEnableBase = null;

    // The On disable base function.
    private LuaFunction m_cOnDisableBase = null;

    // Use this for initialization.
    void Awake()
    {
        if (!YwLuaScriptMng.Instance.Initialized)
        {
            return;
        }

        // Directly creat a lua class instance to associate with this monobehavior.
        if (string.IsNullOrEmpty(m_className) || !CreateClassInstance(m_className))
        {
            return;
        }

        // Initialize parameters need to be passed to lua.
        if ((null != m_parameters) && (m_parameters.Length > 0))
        {
            m_cBehavior.SetData("m_aParameters", m_parameters);
        }

        // Init update flags.
        m_cBehavior.SetData("m_bUpdate", m_monoMethods.Contains(EMonoMethod.Update));
        m_cBehavior.SetData("m_bLateUpdate", m_monoMethods.Contains(EMonoMethod.LateUpdate));
        m_cBehavior.SetData("m_bFixedUpdate", m_monoMethods.Contains(EMonoMethod.FixedUpdate));
        m_cBehavior.SetData("m_bLiteUpdate", m_monoMethods.Contains(EMonoMethod.LiteUpdate));

        // To set custom parameters need to be passed to lua.
        OnSetCustomParameters();

        // Call awake base.
        LuaFunction cAwakeBase = null;
        m_cBehavior.CallMethod(ref cAwakeBase, "AwakeBase", m_cBehavior.GetChunk());

        // Call awake.
        m_cBehavior.Awake();
    }

	// Use this for initialization.
	void Start()
    {
	    if (!m_bReady || !m_monoMethods.Contains(EMonoMethod.Start))
        {
            return;
        }

        m_cBehavior.Start();
    }

    // OnTriggerEnter is called when the Collider other enters the trigger.
    void OnTriggerEnter(Collider cOther)
    {
        if (!m_bReady || !m_monoMethods.Contains(EMonoMethod.OnTriggerEnter))
        {
            return;
        }

        m_cBehavior.OnTriggerEnter(cOther);
    }

    // Sent when another object enters a trigger collider attached to this object (2D physics only).
    void OnTriggerEnter2D(Collider2D cOther)
    {
        if (!m_bReady || !m_monoMethods.Contains(EMonoMethod.OnTriggerEnter2D))
        {
            return;
        }

        m_cBehavior.OnTriggerEnter2D(cOther);
    }

    // OnTriggerExit is called when the Collider other has stopped touching the trigger.
    void OnTriggerExit(Collider cOther)
    {
        if (!m_bReady || !m_monoMethods.Contains(EMonoMethod.OnTriggerExit))
        {
            return;
        }

        m_cBehavior.OnTriggerExit(cOther);
    }

    // Sent when another object leaves a trigger collider attached to this object (2D physics only).
    void OnTriggerExit2D(Collider2D cOther)
    {
        if (!m_bReady || !m_monoMethods.Contains(EMonoMethod.OnTriggerExit2D))
        {
            return;
        }

        m_cBehavior.OnTriggerExit2D(cOther);
    }

    // This function is called when the behaviour becomes disabled or inactive.
    void OnEnable()
    {
        if (!m_bReady)
        {
            return;
        }

        // Call on enable base anyway.
        m_cBehavior.CallMethod(ref m_cOnEnableBase, LUA_FUNC_NAME_ON_ENABLE_BASE, m_cBehavior.GetChunk());

        // Call enable.
        if (m_monoMethods.Contains(EMonoMethod.OnEnable))
        {
            m_cBehavior.OnEnable();
        }
    }

    // This function is called when the object becomes enabled and active.
    void OnDisable()
    {
        if (!m_bReady)
        {
            return;
        }

        // Call on disable base anyway.
        m_cBehavior.CallMethod(ref m_cOnDisableBase, LUA_FUNC_NAME_ON_DISENABLE_BASE, m_cBehavior.GetChunk());

        // Call disable.
        if (m_monoMethods.Contains(EMonoMethod.OnDisable))
        {
            m_cBehavior.OnDisable();
        }
    }

    // On destroy method.
    void OnDestroy()
    {
        if (!m_bReady)
        {
            return;
        }

        // Call on destroy base anyway.
        LuaFunction cOnDestroyBase = null;
        m_cBehavior.CallMethod(ref cOnDestroyBase, "OnDestroyBase", m_cBehavior.GetChunk());

        // Call on destroy.
        if (m_monoMethods.Contains(EMonoMethod.OnDestroy))
        {
            m_cBehavior.OnDestroy();
        }
    }

    /**
     * To set custom parameters need to be passed to lua.
     * 
     * @param void.
     * @return void.
     */
    public virtual void OnSetCustomParameters()
    {
    }
}
