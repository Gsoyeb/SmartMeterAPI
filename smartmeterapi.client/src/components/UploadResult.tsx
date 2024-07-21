import React from 'react';
import { Typography, Box } from '@mui/material';

interface UploadResultProps {
  successfulReads: number;
  failedReads: number;
}

export const UploadResult: React.FC<UploadResultProps> = ({ successfulReads, failedReads }) => {
  return (
    <Box sx={{ mt: 2 }}>
      <Typography variant="body1">
        Successful Reads: {successfulReads}
      </Typography>
      <Typography variant="body1">
        Failed Reads: {failedReads}
      </Typography>
    </Box>
  );
};
