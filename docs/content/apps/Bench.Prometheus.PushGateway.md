+++
title = "Prometheus Push Gateway"
weight = 136
app_library = "default"
app_category = "Monitoring"
app_typ = "default"
app_ns = "Bench.Prometheus"
app_id = "Bench.Prometheus.PushGateway"
app_version = "0.4.0"
app_categories = ["Monitoring"]
app_libraries = ["default"]
app_types = ["default"]
+++

**ID:** `Bench.Prometheus.PushGateway`  
**Version:** 0.4.0  
<!--more-->

[Back to all apps](/apps/)

## Description
The Prometheus Pushgateway exists to allow ephemeral and batch jobs
to expose their metrics to Prometheus.
Since these kinds of jobs may not exist long enough to be scraped,
they can instead push their metrics to a Pushgateway.
The Pushgateway then exposes these metrics to Prometheus.

## Source

* Library: [`default`](/app_libraries/default)
* Category: [Monitoring](/app_categories/monitoring)
* Order Index: 136

## Properties

* Namespace: Bench.Prometheus
* Name: PushGateway
* Typ: `default`
* Website: <https://github.com/prometheus/pushgateway>
* Dependencies: [Prometheus](/apps/Bench.Prometheus)

