import React, { useState, useRef } from 'react';
import { Box, Button, Card, CardContent, Typography, CircularProgress } from '@mui/material';
import { styled } from '@mui/system';
import { UploadResult } from './UploadResult';

const Input = styled('input')({
  display: 'none',
});

const UploadMeterReadings: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const [result, setResult] = useState<{ successfulReads: number; failedReads: number } | null>(null);
  const [loading, setLoading] = useState(false);
  const inputRef = useRef<HTMLInputElement | null>(null);

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      setFile(event.target.files[0]);
      setResult(null);
    }
  };

  const handleUpload = async () => {
    if (!file) return;

    const formData = new FormData();
    formData.append('file', file);

    setLoading(true);

    try {
      const response = await fetch('https://localhost:7269/api/MeterReading/meter-reading-uploads', {
        method: 'POST',
        body: formData,
      });

      if (!response.ok) {
        throw new Error('Failed to upload file');
      }

      const data = await response.json();
      console.log(data);
      setResult(data);
    } catch (error) {
      console.error('Error:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleClear = () => {
    setFile(null);
    setResult(null);
    if (inputRef.current) {
      inputRef.current.value = '';
    }
  };

  return (
    <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
      <Card>
        <CardContent>
          <Typography variant="h5" component="div">
            Upload Meter Readings
          </Typography>
          <label htmlFor="upload-file">
            <Input
              id="upload-file"
              type="file"
              accept=".csv"
              onChange={handleFileChange}
              ref={inputRef}
            />
            <Button variant="contained" component="span" sx={{ mt: 2 }}>
              Choose File
            </Button>
          </label>
          {file && (
            <Typography variant="body1" sx={{ mt: 2 }}>
              Selected file: {file.name}
            </Typography>
          )}
          <Button
            variant="contained"
            color="primary"
            sx={{ mt: 2 }}
            onClick={handleUpload}
            disabled={!file || loading}
          >
            {loading ? <CircularProgress size={24} /> : 'Upload'}
          </Button>
          <Button
            variant="contained"
            color="secondary"
            sx={{ mt: 2, ml: 2 }}
            onClick={handleClear}
          >
            Clear
          </Button>
          {result && <UploadResult successfulReads={result.successfulReads} failedReads={result.failedReads} />}
        </CardContent>
      </Card>
    </Box>
  );
};

export default UploadMeterReadings;
