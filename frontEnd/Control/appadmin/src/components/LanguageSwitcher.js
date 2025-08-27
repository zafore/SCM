import React from 'react';
import { useTranslation } from 'react-i18next';
import {
  FormControl,
  Select,
  MenuItem,
  Box,
  Typography
} from '@mui/material';
import {
  Language as LanguageIcon
} from '@mui/icons-material';

const LanguageSwitcher = () => {
  const { i18n, t } = useTranslation();

  const handleLanguageChange = (event) => {
    const selectedLanguage = event.target.value;
    i18n.changeLanguage(selectedLanguage);
  };

  const languages = [
    { code: 'ar', name: 'العربية', flag: '🇸🇦' },
    { code: 'en', name: 'English', flag: '🇺🇸' }
  ];

  return (
    <Box display="flex" alignItems="center" sx={{ minWidth: 120 }}>
      <LanguageIcon sx={{ mr: 1, fontSize: 20, color: 'white' }} />
      <FormControl size="small" sx={{ minWidth: 100 }}>
        <Select
          value={i18n.language}
          onChange={handleLanguageChange}
          displayEmpty
          sx={{
            color: 'white',
            '& .MuiOutlinedInput-notchedOutline': {
              borderColor: 'rgba(255, 255, 255, 0.3)',
            },
            '&:hover .MuiOutlinedInput-notchedOutline': {
              borderColor: 'rgba(255, 255, 255, 0.5)',
            },
            '&.Mui-focused .MuiOutlinedInput-notchedOutline': {
              borderColor: 'white',
            },
            '& .MuiSelect-select': {
              display: 'flex',
              alignItems: 'center',
              gap: 1,
              color: 'white',
            },
            '& .MuiSvgIcon-root': {
              color: 'white',
            }
          }}
        >
          {languages.map((lang) => (
            <MenuItem key={lang.code} value={lang.code}>
              <Box display="flex" alignItems="center" gap={1}>
                <Typography component="span" sx={{ fontSize: '1.2em' }}>
                  {lang.flag}
                </Typography>
                <Typography variant="body2">
                  {lang.name}
                </Typography>
              </Box>
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </Box>
  );
};

export default LanguageSwitcher;
