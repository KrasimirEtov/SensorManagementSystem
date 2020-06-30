import 'package:conditional_builder/conditional_builder.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:sensor_management_system/models/sensorProperty.dart';
import 'package:sensor_management_system/models/sensor.dart';
import 'package:sensor_management_system/services/webservice.dart';

class CreateSensorForm extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return _CreateSensorFormState();
  }
}

class _CreateSensorFormState extends State<CreateSensorForm> {
  final _formKey = GlobalKey<FormState>();
  Sensor _model = Sensor();
  List<Sensor> _sensors = List<Sensor>();
  List<SensorProperty> _sensorProperties = List<SensorProperty>();
  SensorProperty _selectedSensorPropertyFromDropdown;
  String _tempMinRangeValue = "0";
  String _tempMaxRangeValue = "0";
  String _tempPollingInterval = "0";
  Future _loadSensorProperty;

  @override
  void initState() {
    super.initState();
    _loadSensorProperty = _populateSensorProperties();
    _populateSensors();
  }

  Future _createSensor() {
    return WebService().send(Sensor.initWithJsonBody(_model));
  }

  Future _populateSensorProperties() {
    return WebService().load(SensorProperty.all).then((sensorProperties) => {
          setState(() {
            _sensorProperties = sensorProperties;
            _selectedSensorPropertyFromDropdown = _sensorProperties.first;
            _model.sensorPropertyId = _selectedSensorPropertyFromDropdown.id;
          })
        });
  }

  Future _populateSensors() {
    return WebService().load(Sensor.all).then((sensors) => {
          setState(() => {_sensors = sensors})
        });
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder(
        future: _loadSensorProperty,
        builder: (context, snapshot) {
          if (snapshot.hasData) {
            return Form(
                key: _formKey,
                child: SingleChildScrollView(
                    child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: <Widget>[
                    TextFormField(
                      decoration: InputDecoration(
                          labelText: 'Description',
                          icon: Icon(Icons.description)),
                      validator: (value) {
                        if (value.isEmpty) {
                          return 'Description is required';
                        }
                        if (value.length < 3) {
                          return 'Description must have at least 3 characters!';
                        }
                        return null;
                      },
                      onChanged: (newValue) {
                        _model.description = newValue;
                      },
                    ),
                    TextFormField(
                      keyboardType: TextInputType.numberWithOptions(),
                      decoration: InputDecoration(
                          labelText: 'Polling Interval',
                          icon: Icon(Icons.timer)),
                      validator: (value) {
                        if (value.isEmpty) {
                          return 'Polling Interval is required';
                        }
                        if (int.parse(value) < 0) {
                          return 'Polling Interval should be a positive value!';
                        }
                        if (_sensors.any((e) =>
                            double.parse(e.minRangeValue) ==
                                double.parse(_tempMinRangeValue) &&
                            double.parse(e.maxRangeValue) ==
                                double.parse(_tempMaxRangeValue) &&
                            int.parse(e.pollingInterval) ==
                                int.parse(_tempPollingInterval) &&
                            e.sensorPropertyId ==
                                _selectedSensorPropertyFromDropdown.id)) {
                          return 'Sensor with this Polling Interval, Min and Max Range already exists!';
                        }
                        return null;
                      },
                      onChanged: (newValue) {
                        setState(() {
                          _tempPollingInterval = newValue;
                        });
                        _model.pollingInterval = _tempPollingInterval;
                      },
                    ),
                    DropdownButton<SensorProperty>(
                        hint: Text('Select Sensor Type'),
                        value: _selectedSensorPropertyFromDropdown,
                        items: _sensorProperties.map((e) {
                          return new DropdownMenuItem<SensorProperty>(
                            value: e,
                            child: new Text(e.measureType),
                          );
                        }).toList(),
                        onChanged: (value) {
                          setState(() {
                            _selectedSensorPropertyFromDropdown = value;
                            if (_selectedSensorPropertyFromDropdown.isSwitch
                                    .toLowerCase() ==
                                'true') {
                              _model.minRangeValue = null;
                              _model.maxRangeValue = null;
                            }
                          });
                          _model.sensorPropertyId =
                              _selectedSensorPropertyFromDropdown.id;
                        }),
                    TextFormField(
                      readOnly: _selectedSensorPropertyFromDropdown.isSwitch
                              .toLowerCase() ==
                          'true',
                      keyboardType:
                          TextInputType.numberWithOptions(decimal: true),
                      decoration: InputDecoration(
                          labelText: 'Min Range Value',
                          icon: Icon(Icons.arrow_downward)),
                      validator: (value) {
                        if (_selectedSensorPropertyFromDropdown.isSwitch
                                .toLowerCase() ==
                            'true') {
                          print(_selectedSensorPropertyFromDropdown.isSwitch
                              .toLowerCase());
                          _model.minRangeValue = null;
                          return null;
                        }
                        if (value.isEmpty) {
                          return 'Min Range Value is required';
                        }
                        if (double.parse(value) >
                            double.parse(_tempMaxRangeValue)) {
                          return 'Min Range shold be less than Max Range!';
                        }
                        if (_sensors.any((e) =>
                            double.parse(e.minRangeValue) ==
                                double.parse(_tempMinRangeValue) &&
                            double.parse(e.maxRangeValue) ==
                                double.parse(_tempMaxRangeValue) &&
                            int.parse(e.pollingInterval) ==
                                int.parse(_tempPollingInterval) &&
                            e.sensorPropertyId ==
                                _selectedSensorPropertyFromDropdown.id)) {
                          return 'Sensor with this Polling Interval, Min and Max Range already exists!';
                        }

                        return null;
                      },
                      onChanged: (newValue) {
                        setState(() {
                          _tempMinRangeValue = newValue;
                        });
                        _model.minRangeValue = _tempMinRangeValue;
                      },
                    ),
                    TextFormField(
                      readOnly: _selectedSensorPropertyFromDropdown.isSwitch
                              .toLowerCase() ==
                          'true',
                      keyboardType:
                          TextInputType.numberWithOptions(decimal: true),
                      decoration: InputDecoration(
                          labelText: 'Max Range Value',
                          icon: Icon(Icons.arrow_upward)),
                      validator: (value) {
                        if (_selectedSensorPropertyFromDropdown.isSwitch
                                .toLowerCase() ==
                            'true') {
                          _model.maxRangeValue = null;
                          return null;
                        }
                        if (value.isEmpty) {
                          return 'Max Range Value is required';
                        }
                        if (double.parse(value) <
                            double.parse(_tempMinRangeValue)) {
                          return 'Max Range should be more than Min Range!';
                        }
                        if (_sensors.any((e) =>
                            double.parse(e.minRangeValue) ==
                                double.parse(_tempMinRangeValue) &&
                            double.parse(e.maxRangeValue) ==
                                double.parse(_tempMaxRangeValue) &&
                            int.parse(e.pollingInterval) ==
                                int.parse(_tempPollingInterval) &&
                            e.sensorPropertyId ==
                                _selectedSensorPropertyFromDropdown.id)) {
                          return 'Sensor with this Polling Interval, Min and Max Range already exists!';
                        }
                        return null;
                      },
                      onChanged: (newValue) {
                        setState(() {
                          _tempMaxRangeValue = newValue;
                        });
                        _model.maxRangeValue = _tempMaxRangeValue;
                      },
                    ),
                    Padding(
                      padding: const EdgeInsets.symmetric(vertical: 16.0),
                      child: RaisedButton(
                        onPressed: () async {
                          // Validate returns true if the form is valid, or false
                          // otherwise.
                          if (_formKey.currentState.validate()) {
                            // If the form is valid, display a Snackbar.
                            await _createSensor().then((value) {
                              Scaffold.of(context).showSnackBar(
                                  SnackBar(content: Text('Created sensor')));
                            }).catchError(() {
                              Scaffold.of(context).showSnackBar(SnackBar(
                                  content: Text(
                                      'Error when trying to create sensor')));
                            });
                          }
                        },
                        child: Text('Create'),
                      ),
                    ),
                  ],
                )));
          } else {
            return CircularProgressIndicator();
          }
        });
  }
}
